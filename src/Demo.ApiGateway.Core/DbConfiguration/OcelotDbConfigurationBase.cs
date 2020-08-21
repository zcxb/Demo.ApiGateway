using Demo.ApiGateway.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Demo.ApiGateway.Core.DbConfiguration
{
    public abstract class OcelotDbConfigurationBase
    {
        public abstract Task<(GlobalConfiguration, List<Route>)> GetConfiguration(string connectionString);
    }
}
