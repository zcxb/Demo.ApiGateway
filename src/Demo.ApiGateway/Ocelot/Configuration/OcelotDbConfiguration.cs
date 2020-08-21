using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.ApiGateway.Ocelot.Configuration
{
    public class OcelotDbConfiguration
    {
        public string ConnectionString { get; set; }
        public bool EnableTimer { get; set; } = false;
        public int TimerDelay { get; set; } = 1 * 60 * 1000;
    }
}
