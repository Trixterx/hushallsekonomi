using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace hushallsekonomiClasses.Helpers
{
    /// <summary>
    /// Class with methods handling program loging functions.
    /// </summary>
    public static class Logging
    {
        public static string Filename
        {
            get
            {
                var desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                var filename = Path.Combine(desktop, DateTime.Now.ToString("yyyy-MM-dd") + ".log");
                return filename;
            }
        }

        public static void Log(string text) => File.AppendAllText(Filename, DateTime.Now + " : " + text + "\r\n");

        public static void Log(Exception ex)
        {
            File.AppendAllText(Filename, DateTime.Now + " : " + ex.StackTrace + "\r\n" + ex.Message + "\r\n");
        }

        public static void Log(List<string> text)
        {
            File.AppendAllText(Filename, DateTime.Now + " : " + text + "\r\n");
            File.AppendAllLines(Filename, text);
        }
    }
}
