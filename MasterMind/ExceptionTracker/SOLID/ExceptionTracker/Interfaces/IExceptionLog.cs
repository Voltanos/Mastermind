using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMindLibrary.SOLID.ExceptionTracker.Interfaces
{
    interface IExceptionLog
    {
        void ExceptionLog(string message, string stackTrace);
    }
}
