using ControllerToMouse.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            // nothing, for now
        }
    }
}
