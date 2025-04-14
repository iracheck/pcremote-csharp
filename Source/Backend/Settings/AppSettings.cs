using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json;
using ControllerToMouse.Meta;

namespace ControllerToMouse.Settings
{
    // Singleton class
    internal static class AppSettings
    {
        static string FilePath = FilePaths.AppSettingsDataPath;
        static AppSettingsData Data = new AppSettingsData();

        public static SleepSettings Sleep => Data.SleepSettings;
        public static GUISettings GUI => Data.GUISettings;
        public static AppBehaviorSettings AppBehavior => Data.AppBehaviorSettings;
        public static BlacklistSettings Blacklist => Data.BlacklistSettings;

        // Returns the data source for App Settings
        public static AppSettingsData Get() {  return Data; }

        // File Saving and Loading
        public static void Save()
        {
            try
            {
                string serializedJson = JsonSerializer.Serialize(Data, new JsonSerializerOptions
                {
                    WriteIndented = true,
                });
                File.WriteAllText(FilePath, serializedJson);
            }
            catch (Exception e) 
            {
                Console.WriteLine("There was an error saving AppSettingsData to json. Error Message: " + e.Message);
            }
        }

        public static void Load()
        {
            if (File.Exists(FilePath))
            {
                try
                {
                    string json = File.ReadAllText(FilePath);
                    Data = JsonSerializer.Deserialize<AppSettingsData>(json);
                    return;
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }

            // If the file does not exist, or the file cannot be read from, it will fallback to default settings.
            Console.WriteLine("Failed to retrieve settings, or settings file does not exist. Using default settings...");
            Data = new AppSettingsData();


             return;
        }








        // Data modification
        public static void SetTimeBeforeSleep(int seconds)
        {
            Sleep.TimeBeforeSleep = seconds * 1000; // converts to milliseconds
        }

        public static void SetTimeBeforeSleep(int minutes, int seconds)
        {
            Sleep.TimeBeforeSleep = minutes * 60 * 1000 + seconds * 1000; // converts to milliseconds
        }

        // Blacklist
        public static bool AddToBlacklist(string app)
        {
            if (Blacklist.BlacklistedApplications.Contains(app)) return false;
            else
            {
                Blacklist.BlacklistedApplications.Add(app);
                return true;
            }
        }

        public static bool RemoveFromBlacklist(string app)
        {
            if (!Blacklist.BlacklistedApplications.Contains(app)) return false;
            else
            {
                Blacklist.BlacklistedApplications.Remove(app);
                return true;
            }
        }

        public static bool IsBlacklisted(string app)
        {
            if (Blacklist.BlacklistedApplications.Contains(app))
            {
                return true;
            }
            return false;
        }
    }
}
