using Demo.ApiGateway.Core.Databases;
using Demo.ApiGateway.Extensions;
using Ocelot.Configuration.File;
using Ocelot.Configuration.Repository;
using Ocelot.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.ApiGateway.Ocelot.Configurations
{
    public class FileConfigurationRepositoryBase<TDatabaseConfiguration> : IFileConfigurationRepository
        where TDatabaseConfiguration : OcelotDbConfigurationBase
    {
        private readonly OcelotDatabaseConfiguration _configuration;
        private readonly TDatabaseConfiguration _databaseConfiguration;

        public FileConfigurationRepositoryBase(OcelotDatabaseConfiguration configuration,
                                               TDatabaseConfiguration databaseConfiguration)
        {
            _configuration = configuration;
            _databaseConfiguration = databaseConfiguration;
        }

        public async Task<Response<FileConfiguration>> Get()
        {
            var (globalResult, routeResult) = await _databaseConfiguration.GetConfiguration(_configuration.ConnectionString);

            var file = new FileConfiguration();

            //赋值全局信息
            var global = new FileGlobalConfiguration
            {
                BaseUrl = globalResult.BaseUrl,
                DownstreamScheme = globalResult.DownstreamScheme,
                RequestIdKey = globalResult.RequestIdKey
            };
            if (!string.IsNullOrEmpty(globalResult.HttpHandlerOptions))
            {
                global.HttpHandlerOptions = globalResult.HttpHandlerOptions.ToObject<FileHttpHandlerOptions>();
            }
            if (!string.IsNullOrEmpty(globalResult.LoadBalancerOptions))
            {
                global.LoadBalancerOptions = globalResult.LoadBalancerOptions.ToObject<FileLoadBalancerOptions>();
            }
            if (!string.IsNullOrEmpty(globalResult.QoSOptions))
            {
                global.QoSOptions = globalResult.QoSOptions.ToObject<FileQoSOptions>();
            }
            if (!string.IsNullOrEmpty(globalResult.ServiceDiscoveryProvider))
            {
                global.ServiceDiscoveryProvider = globalResult.ServiceDiscoveryProvider.ToObject<FileServiceDiscoveryProvider>();
            }
            file.GlobalConfiguration = global;

            //提取所有路由信息
            var routes = new List<FileRoute>();
            foreach (var r in routeResult)
            {
                var route = new FileRoute();
                if (!string.IsNullOrEmpty(r.AuthenticationOptions))
                {
                    route.AuthenticationOptions = r.AuthenticationOptions.ToObject<FileAuthenticationOptions>();
                }
                if (!string.IsNullOrEmpty(r.CacheOptions))
                {
                    route.FileCacheOptions = r.CacheOptions.ToObject<FileCacheOptions>();
                }
                if (!string.IsNullOrEmpty(r.DelegatingHandlers))
                {
                    route.DelegatingHandlers = r.DelegatingHandlers.ToObject<List<string>>();
                }
                if (!string.IsNullOrEmpty(r.LoadBalancerOptions))
                {
                    route.LoadBalancerOptions = r.LoadBalancerOptions.ToObject<FileLoadBalancerOptions>();
                }
                if (!string.IsNullOrEmpty(r.QoSOptions))
                {
                    route.QoSOptions = r.QoSOptions.ToObject<FileQoSOptions>();
                }
                if (!string.IsNullOrEmpty(r.DownstreamHostAndPorts))
                {
                    route.DownstreamHostAndPorts = r.DownstreamHostAndPorts.ToObject<List<FileHostAndPort>>();
                }
                //开始赋值
                route.DownstreamPathTemplate = r.DownstreamPathTemplate;
                route.DownstreamScheme = r.DownstreamScheme;
                route.Key = r.RequestIdKey;
                route.Priority = r.Priority ?? 0;
                route.RequestIdKey = r.RequestIdKey;
                route.ServiceName = r.ServiceName;
                route.UpstreamHost = r.UpstreamHost;
                route.UpstreamHttpMethod = r.UpstreamHttpMethod?.ToObject<List<string>>();
                route.UpstreamPathTemplate = r.UpstreamPathTemplate;
                routes.Add(route);
            }
            file.Routes = routes;

            if (file.Routes == null || file.Routes.Count == 0)
            {
                return new OkResponse<FileConfiguration>(null);
            }
            return new OkResponse<FileConfiguration>(file);
        }

        public async Task<Response> Set(FileConfiguration fileConfiguration)
        {
            return new OkResponse();
        }
    }
}
