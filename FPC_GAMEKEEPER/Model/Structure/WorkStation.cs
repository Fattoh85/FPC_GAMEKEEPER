using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPC.Model.Structure
{
    internal class WorkStation
    {
        public string Name { get; set; }
        public string Ip { get; set; }
        public List<User> Users { get; set; }
    }
}
