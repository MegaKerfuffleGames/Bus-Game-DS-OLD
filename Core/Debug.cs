/*
Debug contains useful debug methods
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace BDVR_Dedicated_Server
{
    class Debug
    {
        public static void Log(string _msg)
        {
            Console.WriteLine($"[{DateTimeOffset.Now.ToString("T")}] [INFO] {_msg}");
        }
        public static void LogWarning(string _msg)
        {
            Console.WriteLine($"[{DateTimeOffset.Now.ToString("T")}] [WARNING] {_msg}");
        }
    }
}
