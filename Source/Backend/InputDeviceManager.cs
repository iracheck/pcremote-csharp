using ControllerToMouse.src.Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX.XInput;
using System.Threading.Tasks;

namespace ConsoleApp1.Source.Backend
{
    internal static class InputDeviceManager
    {
        static InputDevice mainController;
        static List<InputDevice> connectedDevices;

        static InputDeviceManager()
        {

        }

        static void getConnectedControllers()
        {
            List<InputDevice> connectedDevices = new List<InputDevice>();
            for (int i = 0; i < 4; i++)
            {
                Controller controller;
            }
        }

        static void updateConnectedControllers()
        {
            for (int i = 0; i < connectedDevices.Count; i++)
            {
                connectedDevices[i].pollDevice();
            }
        }
    }
}
