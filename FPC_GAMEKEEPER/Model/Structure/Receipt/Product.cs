using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPC.Model.Structure.Receipt
{
    public class Product
    {
        public string name { get; set; }

        public long price { get; set; }
        public decimal quantity { get; set; }
        public string commodity { get; set; }
        public string vatCode { get; set; }
        public long sum { get; set; }
    }
}
