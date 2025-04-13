using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerToMouse.Settings
{
    class AppSettingsData
    {
        // Properties that deal with controller sleep and system power use.
        public int ActiveRefreshSpeed { get; set; } = 5;

        public int SleepRefreshSpeed { get; set; } = 100;

        public int BatterySaverRefreshSpeed { get; set; } = 20;

        public int TimeBeforeSleep { get; set; } = 30000; // 30 seconds

        public bool IdleSleepEnabled { get; set; } = true;

        public bool BatterySaverEnabled { get; set; } = true;

        // App behavior
        public bool OpenOnStartup { get; set; } = false;
        public bool CheckForUpdates { get; set; } = true;
        public bool MinimizeToTray { get; set; } = true;


        // Blacklist settings
        public bool BlacklistEnabled { get; set; } = false;

        public List<string> BlacklistedApplications { set; get; } = new List<string>();


        // Media Control settings
        public int AudioStep { get; set; } = 5; // in percentage, 0-100
    }
}
