using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPC.Model.Structure.Receipt
{
    public class Taxes
    {
        public List<Vat> vats { get; set; } = new List<Vat>();


        public Taxes() {
            vats = new List<Vat>();
        }
    }
}
