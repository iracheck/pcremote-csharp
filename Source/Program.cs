using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Threading;

using ControllerToMouse.Devices;
using ControllerToMouse.Utils;
using ControllerToMouse.Meta;
using ControllerToMouse.Settings;

using WindowsInput.Native;
using SharpDX.XInput;

namespace ControllerToMouse
{
    internal class Program
    {
        static void Main(string[] args)
        {
            FilePaths.EnsureDirectoriesExist();

            InputDeviceManager.InitializeDevices();

            AppSettings.Load();
            AppSettings.Save();

            Console.WriteLine(AppSettings.Sleep.TimeBeforeSleep);
        }
    }
}
