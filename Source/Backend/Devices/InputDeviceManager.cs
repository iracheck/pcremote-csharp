using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ControllerToMouse.Settings;
using ControllerToMouse.Devices;
using SharpDX.XInput;

namespace ControllerToMouse.Devices
{
    static class InputDeviceManager
    {
        // Keep track of all four maximum connected devices
        static InputDevice Device1 = null;
        static InputDevice Device2 = null;
        static InputDevice Device3 = null;
        static InputDevice Device4 = null;


        static InputDeviceManager()
        {
            try
            {
                Device1 = new InputDevice();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static int CheckForDevices()
        {
            try
            {
                if (Device1 == null) Device1 = new InputDevice(UserIndex.One);
                if (Device2 == null) Device2 = new InputDevice(UserIndex.Two);
                if (Device2 == null) Device2 = new InputDevice(UserIndex.Three);
                if (Device2 == null) Device2 = new InputDevice(UserIndex.Four);

            }
            return -1;
        }

    }
}
