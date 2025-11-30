using ControllerToMouse.Devices;
using ControllerToMouse.Source.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerToMouse.Source.GUI
{
    internal class ReloadControllersAction : ctmAction
    {
         
        public ReloadControllersAction()
        {
            Name = "Reload Devices";
            Description = "Fetch newly connected devices and reload existing ones.";
        }

        public override void Activate()
        {
            InputDeviceManager.InitializeDevices();
            StatusBar.Message("Refreshed all controllers. Found " + InputDeviceManager.ConnectedDeviceCount() + " connected devices.");
        }
    }
}
