using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace brokenaccesscontrol.Utils
{
    [Serializable]
    public class FileLog
    {
        private string path;
        private string initialContent = "# Log Started";

        public  FileLog(string path){
            this.path = path;
            Log(initialContent);
        }

        public void Log(string message){
            using (StreamWriter streamWriter = File.AppendText(path)){
                streamWriter.WriteLine(message);
            }

        }
        [OnDeserialized()]
        internal void OnDeserializedMethod(StreamingContext context){
            Log(initialContent);
        }

    }
}