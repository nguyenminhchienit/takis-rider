using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Infrastructure.Shared.Discovery.Request
{
    public class DiscoveryRequest
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int Port { get; set; }
    }
}
