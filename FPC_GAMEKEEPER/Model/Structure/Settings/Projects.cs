using FPC.Model.Structure.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPC.Model.Structure
{
    public class ModuleSettings
    {
        public int tranCheckDelay { get;  set; }
        public int printerCheckDelay { get;  set; }
        public int posId { get;  set; }
        public string connectionString { get; set; }
        public string dbName { get; set; }
        public string Ip { get; set; }
        public bool status { get; set; }
        public bool flibber { get; set; }
        public string vatCode { get; set; }
        public string commodity { get; set; }
        public List<User> Users { get; set; }
        public string startDate { get; set; }
        

        public List<VatCode> vatCodeList { get; set; }
    }
}
