using Demo.ApiGateway.Core.DbConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.ApiGateway.Ocelot.Configuration.FileConfigurationRepository
{
    public class MySqlFileConfigurationRepository : FileConfigurationRepositoryBase<MySqlOcelotDbConfiguration>
    {
        public MySqlFileConfigurationRepository(OcelotDbConfiguration configuration, MySqlOcelotDbConfiguration databaseConfiguration) : base(configuration, databaseConfiguration)
        {
        }
    }
}
