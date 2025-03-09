using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Infrastructure.Shared.Models.Location.Request
{
    public class FindNearestDriverModel
    {
        public double latitude {  get; set; }

        public double longitude { get; set; }

        public double radius { get; set; }
    }
}
