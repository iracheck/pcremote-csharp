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


        public static void AddApplicationToStartup()
        {
            if (!CheckIsAutoStartup())
            {
                String exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                File.Copy(exePath, Path.Combine(StartupAppsPath, "PCRemote.exe"));
            }
        }

        public static void RemoveApplicationFromStartup()
        {
            if (CheckIsAutoStartup())
            {
                File.Delete(Path.Combine(StartupAppsPath, "PCRemote.exe"));
            }
        }

        public static bool CheckIsAutoStartup()
        {
            return File.Exists(Path.Combine(StartupAppsPath, "PCRemote.exe"));
        }
    }
}
