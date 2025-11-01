using System.Windows;
using System.Diagnostics;

using ControllerToMouse.Devices;
using SharpDX.XInput;
using System;
namespace ControllerToMouse.GUI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected void OnExit(object sender, EventArgs e)
        {
            InputDeviceManager.RemoveAllDevices();
            base.OnClosed(e);
        }

        protected void MinimizeButton_Click(object sender, EventArgs e)
        {

        }

        protected void CloseButton_Click(object sender, EventArgs e)
        {

        }

    }
}
