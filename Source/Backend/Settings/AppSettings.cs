using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json;

namespace ControllerToMouse.Settings
{
    // Singleton class
    internal static class AppSettings
    {
        static string FilePath = "settings/appsettings.json";
        static AppSettingsData Data = new AppSettingsData();

        // Returns the data source for App Settings
        public static AppSettingsData Get() {  return Data; }

        // File Saving and Loading
        public static void SaveToFile()
        {
            try
            {
                string serializedJson = JsonSerializer.Serialize(Data);
                File.WriteAllText(FilePath, serializedJson);
            }
            catch (Exception e) 
            {
                Console.WriteLine("There was an error saving AppSettingsData to json. Error Message: " + e.Message);
            }
        }






        // Data modification
        public static void SetTimeBeforeSleep(int seconds)
        {
            Get().TimeBeforeSleep = seconds * 1000; // converts to milliseconds
        }

        public static void SetTimeBeforeSleep(int minutes, int seconds)
        {
            Get().TimeBeforeSleep = minutes * 60 * 1000 + seconds * 1000; // converts to milliseconds
        }

        // Blacklist
        public static bool AddToBlacklist(string app)
        {
            if (Get().BlacklistedApplications.Contains(app)) return false;
            else
            {
                Get().BlacklistedApplications.Add(app);
                return true;
            }
        }

        public static bool RemoveFromBlacklist(string app)
        {
            if (!Get().BlacklistedApplications.Contains(app)) return false;
            else
            {
                Get().BlacklistedApplications.Remove(app);
                return true;
            }
        }

        public static bool IsBlacklisted(string app)
        {
            if (Get().BlacklistedApplications.Contains(app))
            {
                return true;
            }
            return false;
        }
    }
}
