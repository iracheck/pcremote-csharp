using ControllerToMouse.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ControllerToMouse.Source.GUI
{
    class VibrationTest : ctmAction
    {
        public VibrationTest()
        {
            Name = "Vibration Test";
            Description = "Vibrate each controller in succession to verify their order.";
        }

        public override void Activate()
        {
            for (int i = 0; i < InputDeviceManager.ConnectedDeviceCount(); i++)
            {
                InputDevice device = InputDeviceManager.GetDevice(i);

                if (device != null)
                {
                    device.Vibrate();
                }
            }
        }
    }
}
