using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ControllerToMouse.Meta;

namespace ControllerToMouse.Settings
{
    class AppSettingsData
    {
        public SleepSettings SleepSettings { get; set; } = new();
        public GUISettings GUISettings { get; set; } = new();
        public AppBehaviorSettings AppBehaviorSettings { get; set; } = new();
        public BlacklistSettings BlacklistSettings { get; set; } = new();
        public AudioControlSettings AudioControlSettings { get; set; } = new();
    }

    // Properties that deal with controller sleep and system power use.
    internal class SleepSettings
    {
        public int ActiveRefreshSpeed { get; set; } = 5;
        public int SleepRefreshSpeed { get; set; } = 100;
        public int BatterySaverRefreshSpeed { get; set; } = 20;
        public int TimeBeforeSleep { get; set; } = 30000; // 30 seconds
        public bool IdleSleepEnabled { get; set; } = true;
        public bool BatterySaverEnabled { get; set; } = true;
    }

    internal class BlacklistSettings
    {
        public bool BlacklistEnabled { get; set; } = false;
        public bool InvertBlacklistToWhitelist { get; set; } = false;

        public List<string> BlacklistedApplications { set; get; } = new List<string>();
    }

    internal class AppBehaviorSettings
    {
        public bool OpenOnStartup { get; set; } = false;
        public bool CheckForUpdates { get; set; } = true;
        public bool MinimizeToTray { get; set; } = true;
    }

    internal class GUISettings
    {
        public string Theme { get; set; } = "no_theme"; // current avaliable themes: "dark"
    }

    // will (eventually) be migrated onto a per-profile basis
    internal class AudioControlSettings
    {
        public int AudioStep { get; set; } = 5; // in percentage, 0-100
    }
}
