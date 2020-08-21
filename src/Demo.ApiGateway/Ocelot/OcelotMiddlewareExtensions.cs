using Demo.ApiGateway.Core.DbConfiguration;
using Demo.ApiGateway.Ocelot.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ocelot.Configuration;
using Ocelot.Configuration.Creator;
using Ocelot.Configuration.Repository;
using Ocelot.DependencyInjection;
using Ocelot.Logging;
using Ocelot.Middleware;
using Ocelot.Responses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.ApiGateway.Ocelot
{
    public static class OcelotMiddlewareExtensions
    {
        public static async Task<IApplicationBuilder> UseOcelot(this IApplicationBuilder builder)
        {
            await builder.UseOcelot(new OcelotPipelineConfiguration());
            return builder;
        }

        public static async Task<IApplicationBuilder> UseOcelot(this IApplicationBuilder builder, Action<OcelotPipelineConfiguration> pipelineConfiguration)
        {
            var config = new OcelotPipelineConfiguration();
            pipelineConfiguration?.Invoke(config);
            return await builder.UseOcelot(config);
        }

        public static async Task<IApplicationBuilder> UseOcelot(this IApplicationBuilder builder, OcelotPipelineConfiguration pipelineConfiguration)
        {
            var configuration = await CreateConfiguration(builder);

            ConfigureDiagnosticListener(builder);

            return CreateOcelotPipeline(builder, pipelineConfiguration);
        }

        public static Task<IApplicationBuilder> UseOcelot(this IApplicationBuilder app, Action<IApplicationBuilder, OcelotPipelineConfiguration> builderAction)
            => UseOcelot(app, builderAction, new OcelotPipelineConfiguration());

        public static async Task<IApplicationBuilder> UseOcelot(this IApplicationBuilder app, Action<IApplicationBuilder, OcelotPipelineConfiguration> builderAction, OcelotPipelineConfiguration configuration)
        {
            await CreateConfiguration(app);

            ConfigureDiagnosticListener(app);

            builderAction?.Invoke(app, configuration ?? new OcelotPipelineConfiguration());

            app.Properties["analysis.NextMiddlewareName"] = "TransitionToOcelotMiddleware";

            return app;
        }

        private static void ConfigureDiagnosticListener(IApplicationBuilder builder)
        {
            var env = builder.ApplicationServices.GetService<IWebHostEnvironment>();
            var listener = builder.ApplicationServices.GetService<OcelotDiagnosticListener>();
            var diagnosticListener = builder.ApplicationServices.GetService<DiagnosticListener>();
            diagnosticListener.SubscribeWithAdapter(listener);
        }

        private static IApplicationBuilder CreateOcelotPipeline(IApplicationBuilder builder, OcelotPipelineConfiguration pipelineConfiguration)
        {
            builder.BuildOcelotPipeline(pipelineConfiguration);
            builder.Properties["analysis.NextMiddlewareName"] = "TransitionToOcelotMiddleware";
            return builder;
        }

        public static IOcelotBuilder ConfigureOcelotFromDatabase<TDatabaseConfiguration, TFileConfigurationRepository>
            (this IOcelotBuilder builder, Action<OcelotDbConfiguration> option)
            where TDatabaseConfiguration : OcelotDbConfigurationBase
            where TFileConfigurationRepository : class, IFileConfigurationRepository
        {
            builder.Services.Configure(option);
            //配置信息
            builder.Services.AddSingleton(
                resolver => resolver.GetRequiredService<IOptions<OcelotDbConfiguration>>().Value);
            builder.Services.AddSingleton<TDatabaseConfiguration>();
            //配置文件仓储注入
            builder.Services.AddSingleton<TFileConfigurationRepository>();
            builder.Services.AddSingleton<IFileConfigurationRepository, TFileConfigurationRepository>();
            //定时同步
            builder.Services.AddHostedService<DbConfigurationPoller>();
            return builder;
        }

        private static async Task<IInternalConfiguration> CreateConfiguration(IApplicationBuilder builder)
        {
            //提取文件配置信息
            var fileConfig = await builder.ApplicationServices.GetService<IFileConfigurationRepository>().Get();
            var internalConfigCreator = builder.ApplicationServices.GetService<IInternalConfigurationCreator>();
            var internalConfig = await internalConfigCreator.Create(fileConfig.Data);
            //如果配置文件错误直接抛出异常
            if (internalConfig.IsError)
            {
                ThrowToStopOcelotStarting(internalConfig);
            }

            //配置信息缓存，这块需要注意实现方式，因为后期我们需要改造下满足分布式架构,这篇不做讲解
            var internalConfigRepo = builder.ApplicationServices.GetService<IInternalConfigurationRepository>();
            internalConfigRepo.AddOrReplace(internalConfig.Data);
            return GetOcelotConfigAndReturn(internalConfigRepo);
        }

        private static IInternalConfiguration GetOcelotConfigAndReturn(IInternalConfigurationRepository provider)
        {
            var ocelotConfiguration = provider.Get();

            if (ocelotConfiguration?.Data == null || ocelotConfiguration.IsError)
            {
                ThrowToStopOcelotStarting(ocelotConfiguration);
            }

            return ocelotConfiguration.Data;
        }

        private static void ThrowToStopOcelotStarting(Response config)
        {
            throw new Exception($"Unable to start Ocelot, errors are: {string.Join(",", config.Errors.Select(x => x.ToString()))}");
        }
    }
}
