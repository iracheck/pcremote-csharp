using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerToMouse.Settings
{
    // Singleton class
    internal static class GlobalSettings
    {
        // Properties that deal with controller sleep and system power use.
        public static int ActiveRefreshSpeed { get; set; } = 5;

        public static int SleepRefreshSpeed { get; set; } = 100;

        public static int BatterySaverRefreshSpeed { get; set; } = 20;

        public static int TimeBeforeSleep { get; set; } = 30000; // 30 seconds

        public static bool IdleSleepEnabled { get; set; } = true;

        public static bool BatterySaverEnabled { get; set; } = true;


        // Mouse Settings
        public static int MouseSensitivity { get; set; } = 2000;


        // Blacklist settings
        public static bool BlacklistEnabled { get; set; } = false;


        // Media Control settings
        public static float AudioStep { get; set; } = 5.0f;


        // GUI
        public static bool DarkMode { get; set; } = false;



        // Methods begin

        public static void SetTimeBeforeSleep(int seconds)
        {
            TimeBeforeSleep = seconds * 1000; // converts to milliseconds
        }

        public static void SetTimeBeforeSleep(int minutes, int seconds)
        {
            TimeBeforeSleep = minutes * 60 * 1000 + seconds * 1000; // converts to milliseconds
        }

        public static List<string> BlacklistedApplications { get; } = new List<string>();

        public static bool AddToBlacklist(string app)
        {
            if (BlacklistedApplications.Contains(app)) return false;
            else
            {
                BlacklistedApplications.Add(app);
                return true;
            }
        }

        public static bool RemoveFromBlacklist(string app)
        {
            if (!BlacklistedApplications.Contains(app)) return false;
            else
            {
                BlacklistedApplications.Remove(app);
                return true;
            }
        }

        public static bool IsInBlacklist(string app)
        {
            if (BlacklistedApplications.Contains(app))
            {
                return true;
            }
            return false;
        }
    }
}
