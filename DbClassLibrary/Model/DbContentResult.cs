using DbClassLibrary.Model.Sturcure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbClassLibrary.Model
{
    internal class DbContentResult
    {
        public RequestContentResult RequestContentResult { get; set; }
        public DataSet DataSet { get; set; }
    }
}
