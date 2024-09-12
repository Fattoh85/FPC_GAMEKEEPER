using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPC.Model.Enum
{
    internal enum RequestType
    {
        DEVICE_STATUS = 1,
        CLOSE_SHIFT = 2,
        PRINT_LAST_FD = 3,
        PRINT_FD_BY_NUMBER = 4,
        GET_X_REPORT = 5,
        OPEN_SHIFT = 6,
        OPEN_DRAWER = 7,
    }
}
