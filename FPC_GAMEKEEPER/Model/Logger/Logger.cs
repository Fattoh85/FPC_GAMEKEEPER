using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPC.Model.Logger
{
    public class Logger
    {
        public static ILog Get(Type declaringType)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(declaringType);
            log4net.GlobalContext.Properties["pid"] = Process.GetCurrentProcess().Id;
            return log;
        }
    }
}
