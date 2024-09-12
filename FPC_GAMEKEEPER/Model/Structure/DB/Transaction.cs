using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPC.Model.Structure.Trans
{
    public class Transaction
    {
        public string trans_oid { get; internal set; }
        public string trans_date { get; internal set; }
        public string user_id { get; internal set; }
        public string fiscal_receipt_printed { get; internal set; }
    }
}
