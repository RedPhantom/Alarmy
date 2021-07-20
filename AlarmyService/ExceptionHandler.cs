using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlarmyService
{
    //static class ExceptionHandler
    //{
    //    private const string ConnectionRefused = "No connection could be made because the target machine actively refused it";
    //    private const string ForciblyClosed = "An existing connection was forcibly closed by the remote host";

    //    private static Dictionary<string, Type> MessageToException = new Dictionary<string, Type>() {
    //        {ConnectionRefused, typeof(ConnectionRefusedException) },
    //        {ForciblyClosed, typeof(ConnectionForciblyClosedException) }
    //    };

    //    private static string ClassifyException(Exception e)
    //    {
    //        if (e.Message.Contains(ConnectionRefused))
    //        {
    //            return ConnectionRefused;
    //        }
    //        else if (e.Message.Contains(ForciblyClosed))
    //        {
    //            return ForciblyClosed;
    //        }

    //        return string.Empty;
    //    }
    //    public static Type RaiseCustomException(Exception e)
    //    {
    //        foreach (KeyValuePair<string, Type> entry in MessageToException)
    //        {
    //            if (e.Message.Contains(entry.Key))
    //            {
    //                return entry.Value; 
    //            }
    //        }
    //    }
    //}

    //class ConnectionRefusedException : Exception
    //{}

    //class ConnectionForciblyClosedException : Exception
    //{ }
}
