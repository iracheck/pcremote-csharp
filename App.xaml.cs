using ControllerToMouse.Devices;
using ControllerToMouse.Meta;
using ControllerToMouse.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;

namespace ControllerToMouse
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // note: MainWindow.cs will also immediately happen! Frontend logic is mostly there, while backend logic is here.

            // Ensure all program directories exist in %appdata%
            FilePaths.EnsureDirectoriesExist();

            // Save and reload the app settings, to ensure they exist and are valid
            AppSettings.Load();
            AppSettings.Save();

            // Get any connected xinput devices
            InputDeviceManager.InitializeDevices();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            InputDeviceManager.RemoveAllDevices();
            base.OnExit(e);
        }
    }
}
