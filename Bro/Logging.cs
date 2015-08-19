using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bro
{
    public static class Logging
    {
        private static string _logPath = "log.txt";

        public static void WriteToLog(string text)
        {
            text = DateTime.Now + "\r\n" + text + "\r\n\r\n";
            File.AppendAllText(_logPath, text);
        }
    }
}
