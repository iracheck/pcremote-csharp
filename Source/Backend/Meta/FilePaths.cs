using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerToMouse.Meta
{
    static class FilePaths
    {
        // Rename this when the app releases.
        private const string AppName = "Controller To Mouse";

        public static string BaseAppPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppName);

        public static string SettingsDirectory = Path.Combine(BaseAppPath, "Settings");
        public static string AppSettingsDataPath = Path.Combine(SettingsDirectory, "AppSettings.json");

        public static string ProfilesDirectory = Path.Combine(BaseAppPath, "Profiles");

        public static void EnsureDirectoriesExist()
        {
            Directory.CreateDirectory(BaseAppPath);
            Directory.CreateDirectory(SettingsDirectory);
            Directory.CreateDirectory(ProfilesDirectory);
        }
    }
}
