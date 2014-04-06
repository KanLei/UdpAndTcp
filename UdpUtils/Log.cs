using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.IO;

namespace UdpUtils
{
    /// <summary>
    /// Write Log
    /// </summary>
    public class Log
    {
        /// <summary>
        /// Write Log to Log.txt
        /// </summary>
        public static void Write(string text,
            [CallerMemberName] string name = "",
            [CallerFilePath] string path = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            StringBuilder errorInfo = new StringBuilder();
            errorInfo.AppendFormat("Error Info: {0}", text);
            errorInfo.Append(Environment.NewLine);
            errorInfo.AppendFormat("Member Name: {0}", name);
            errorInfo.Append(Environment.NewLine);
            errorInfo.AppendFormat("File Path: {0}", path);
            errorInfo.Append(Environment.NewLine);
            errorInfo.AppendFormat("Line Number: {0}", lineNumber);

            WriteToFile(errorInfo.ToString());
        }

        private static void WriteToFile(string errorInfo)
        {
            using (FileStream fs = new FileStream("Log.txt", FileMode.OpenOrCreate, FileAccess.Write))
            {
                StreamWriter write = new StreamWriter(fs);
                write.WriteLine(errorInfo + Environment.NewLine);
            }
        }
    }
}
