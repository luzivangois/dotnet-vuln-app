using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetVulnApp.Utils
{
    public class OptimisedIO
    {
        public static string readFile(string fileName){
            string content = System.Text.Encoding.Default.GetString(readFileRaw(fileName));
            return content;
        }

        public static byte[] readFileRaw(string fileName){
            byte[] contentBytes = System.IO.File.ReadAllBytes(fileName);
            return contentBytes;
        }

        public static bool saveFileRaw (string fileName, byte[] fileContent){
            System.IO.File.WriteAllBytes(fileName, fileContent);
            return true;
        }
    }
}