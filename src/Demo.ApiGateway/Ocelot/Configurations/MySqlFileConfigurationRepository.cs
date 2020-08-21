using Demo.ApiGateway.Core.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.ApiGateway.Ocelot.Configurations
{
    public class MySqlFileConfigurationRepository : FileConfigurationRepositoryBase<MySqlOcelotDbConfiguration>
    {
        public MySqlFileConfigurationRepository(OcelotDatabaseConfiguration configuration, MySqlOcelotDbConfiguration databaseConfiguration) : base(configuration, databaseConfiguration)
        {
        }
    }
}
