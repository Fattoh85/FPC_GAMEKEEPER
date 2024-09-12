using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPC.Model.Structure
{
    internal class Project
    {
        public string Name { get; set; }
        public bool WorkStationsHasOwnNames { get; set; }
        public List<WorkStation> WorkStations { get; set; }
    }
}
