using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.ApiGateway.Core.DbConfiguration;
using Demo.ApiGateway.Ocelot;
using Demo.ApiGateway.Ocelot.Configuration.FileConfigurationRepository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;

namespace Demo.ApiGateway
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOcelot().ConfigureOcelotFromDatabase<MySqlOcelotDbConfiguration, MySqlFileConfigurationRepository>(options =>
            {
                options.ConnectionString = "Server=localhost; Port=3306; Database=test; Uid=root; Pwd=123456;";
                options.EnableTimer = true;
                options.TimerDelay = 10 * 1000;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseOcelot().Wait();
        }
    }
}
