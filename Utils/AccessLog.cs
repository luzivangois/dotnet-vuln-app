using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace brokenaccesscontrol.Utils
{
    public class AccessLog
    {
        public static string _appLogPath = "logs/Access.log";

        public static void writeLine(string level, string message){
            var line = $"[{DateTime.Now}] [{level}] {message.Replace('\n','_').Replace('\r','_').Replace('\t','_')}" + Environment.NewLine;
            File.AppendAllText(_appLogPath, line);

        }

        public static void Debug(string message){
            writeLine("DEBUG", message);
        }

        public static void Info(string message){
            writeLine("INFO", message);
        }

        public static void Error(string message){
            writeLine("ERROR", message);
        }

        public static void Warning(string message){
            writeLine("WARNING", message);
        }
    }
}