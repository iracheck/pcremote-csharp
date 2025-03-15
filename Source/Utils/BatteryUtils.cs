using System;
using System.Runtime.InteropServices;

namespace ControllerToMouse.Source.Utils
{
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
            int ac = getCurrentStatus().ACLineStatus;
            if (ac == 1) return true;
            else return false;
        }

        public static bool IsOnBatterySaver()
        {
            int bsvr = getCurrentStatus().BatteryFlag;
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
