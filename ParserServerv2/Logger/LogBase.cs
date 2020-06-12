using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserServerv2.Logger
{
    public abstract class LogBase
    {
        public abstract void Log(string message);
    }

    public class FileLogger : LogBase
    {
        public string filePath= @"C:\Users\Administrator\Desktop\ArslanTCP\ArslanLogger.txt";
 
        public override void Log(string message)
        {
            using (StreamWriter streamWriter= new StreamWriter(filePath, true))
            {
                streamWriter.WriteLine(message);
                streamWriter.Close();
            }
        }
    }
}
