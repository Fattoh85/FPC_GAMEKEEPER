using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPC.Model.Structure
{
    public class Project0
    {
        public string connectionString { get; set; }
        public string startDate { get; set; }

        public string Name { get; set; }
        public bool WorkStationsHasOwnNames { get; set; }
        public List<WorkStation> WorkStations { get; set; }
    }
}
