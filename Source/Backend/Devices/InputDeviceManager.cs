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
        public const int MAX_CONTROLLERS = 4;

        // Keep track of all four maximum connected devices
        static Dictionary<UserIndex, InputDevice> ConnectedDevices = new Dictionary<UserIndex, InputDevice>();
        static Dictionary<UserIndex, Thread> DeviceThreads = new Dictionary<UserIndex, Thread>();


        // Search for connected XInput devices, and initialize them if possible.
        public static void InitializeDevices()
        {
            for (int i = 0; i < MAX_CONTROLLERS; i++)
            {
                UserIndex index = (UserIndex)i; // Remember this for later! Cast an enum (UserIndex) using an int
                InputDevice device = new InputDevice(index);

                if (!ConnectedDevices.ContainsKey(index) && device.GetIsConnected())
                {
                    Console.WriteLine("Creating new device with index {0}", index);

                    ConnectedDevices[index] = device;

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

        // Safely removes a device from circulation, freeing up the thread.
        public static int RemoveDevice(UserIndex index)
        {
            if (!ConnectedDevices.ContainsKey(index) || !DeviceThreads.ContainsKey(index))
            {
                return -1;
            }

            ConnectedDevices[index].ShutdownDeviceThread();
            ConnectedDevices.Remove(index);

            DeviceThreads[index].Join();
            DeviceThreads.Remove(index);

            return 0;
        }

        // Important to in all cases use this in a try catch statement. If the controller does not exist it will throw an error
        public static InputDevice GetDevice(UserIndex index)
        {
            if (index == UserIndex.Any)
            {
                return ConnectedDevices[0];
            }

            if (ConnectedDevices.ContainsKey(index))
            {
                return ConnectedDevices[index];
            }
            else
            {
                return null;
            }
        }

        public static InputDevice GetDevice(int intIndex)
        {
            UserIndex index = (UserIndex)intIndex;

            if (ConnectedDevices.ContainsKey(index))
            {
                return ConnectedDevices[index];
            }
            else
            {
                return null;
            }
        }

        public static void RemoveAllDevices()
        {
            for (int i = 0; i < MAX_CONTROLLERS; i++)
            {
                RemoveDevice((UserIndex)i); // safely removes a device
            }
        }

        public static int GetDeviceCount()
        {
            return ConnectedDevices.Count;
        }
    }
}