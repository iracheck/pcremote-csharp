using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerToMouse.Source.Utils
{
    static class StartupUtils
    {
        static String StartupAppsPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);


        public static void AddToStartup()
        {
            
        }

        public static void RemoveFromStartup()
        {

        }

        public static void CheckIsAutoStartup()
        {
            File.Exists(Path.Combine(StartupAppsPath, "PCRemote.exe"));
        }
    }
}
