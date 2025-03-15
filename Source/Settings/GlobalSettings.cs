using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerToMouse.Settings
{
    internal static class GlobalSettings
    {
        // Settings that deal with controller sleep and system power use.
        public static int ActiveRefreshSpeed { get; set; } = 10;

        public static int SleepRefreshSpeed { get; set; } = 100;

        public static int BatterySaverRefreshSpeed { get; set; } = 50;

        public static int TimeBeforeSleep { get; set; } = 500;

        public static bool IdleSleepEnabled { get; set; } = true;

        public static bool BatterySaverEnabled { get; set; } = true;


        // Mouse Settings
        public static int MouseSensitivity { get; set; } = 2000;


        // Blacklist settings
        public static bool BlacklistEnabled { get; set; } = false;

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
