using System;
using System.Runtime.InteropServices;

namespace ControllerToMouse.Utils
{
    // This class handles system and controller-level battery
    internal static class BatteryUtils
    {
        // Imported class from Windows API
        [StructLayout(LayoutKind.Sequential)]
        private struct SYSTEM_POWER_STATUS
        {
            public byte ACLineStatus;
            public byte BatteryFlag;
            public byte BatteryLifePercent;
            public byte SystemStatusFlag;
            public int BatteryLifeTime;
            public int BatteryFullLifeTime;
        }

        [DllImport("kernel32.dll")]
        private static extern bool GetSystemPowerStatus(out SYSTEM_POWER_STATUS power_status);

        private static SYSTEM_POWER_STATUS getCurrentStatus()
        {
            SYSTEM_POWER_STATUS powerStatus;

            GetSystemPowerStatus(out powerStatus);

            return powerStatus;
        }

        public static bool IsOnAC()
        {
            int ac = getCurrentStatus().ACLineStatus; // returns 0 if on battery, 1 if on AC
            if (ac == 1) return true;
            else return false;
        }

        public static bool IsOnBatterySaver()
        {
            int bsvr = getCurrentStatus().SystemStatusFlag; // returns 0 if off, 1 if on
            if (bsvr == 1) return true;
            else return false;
        }

        public static int GetBatteryLevel()
        {
            int lvl = getCurrentStatus().BatteryLifePercent;
            return lvl;
        }
    }
}
