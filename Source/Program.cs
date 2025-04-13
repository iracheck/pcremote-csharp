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
using WindowsInput.Native;
using ControllerToMouse.Settings;
using SharpDX.XInput;

namespace ControllerToMouse
{
    internal class Program
    {
        static void Main(string[] args)
        {
            InputDeviceManager.InitializeDevices();

            AppSettings.SaveToFile();
        }
    }
}
