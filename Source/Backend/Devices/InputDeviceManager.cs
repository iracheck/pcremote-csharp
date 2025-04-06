using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using ControllerToMouse.Settings;
using ControllerToMouse.Devices;
using SharpDX.XInput;

namespace ControllerToMouse.Devices
{
    static class InputDeviceManager
    {
        // Keep track of all four maximum connected devices
        static Dictionary<UserIndex, InputDevice> ConnectedDevices = new Dictionary<UserIndex, InputDevice>();
        static Dictionary<UserIndex, Thread> DeviceThreads = new Dictionary<UserIndex, Thread>();

        static InputDevice Device1;
        static InputDevice Device2;
        static InputDevice Device3;
        static InputDevice Device4;


        // Search for connected XInput devices, and initialize them if possible.
        public static void InitializeDevices()
        {
            for (int i = 0; i < 4; i++)
            {
                UserIndex index = (UserIndex)i; // Remember this for later! Cast an enum (UserIndex) using an int
                InputDevice device = new InputDevice(index);

                if (!ConnectedDevices.ContainsKey(index) && device.GetIsConnected())
                {
                    Console.WriteLine("Creating new device with index {0}", index);

                    // Assert that the device is connected
                    ConnectedDevices[index] = device;

                    // Spin up a new thread
                    Thread handlerThread = new Thread(device.BeginDeviceThread);
                    DeviceThreads[index] = handlerThread;
                    handlerThread.Start();
                }
                else if (ConnectedDevices.ContainsKey(index) && device.GetIsConnected() == false)
                {
                    Console.WriteLine("Device with index {0} is not connected. Removing device...", index);
                    RemoveDevice(index);
                }

                // may need to implement some more robust checking to ensure that a thread does not exist for a controller that technically doesn't
            }
        }

        // Safely removes a device from circulation, freeing up not only the thread but also removing 
        public static int RemoveDevice(UserIndex index)
        {
            if (!ConnectedDevices.ContainsKey(index))
            {
                return 0;
            }

            ConnectedDevices.Remove(index);

            DeviceThreads[index].Join();
            DeviceThreads.Remove(index);

            return 1;
        }
    }
}
