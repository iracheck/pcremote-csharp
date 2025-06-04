using System.Windows;
using System.Diagnostics;

using ControllerToMouse.Devices;
using SharpDX.XInput;
using ControllerToMouse.GUI.UserControls;
using System;
namespace ControllerToMouse.GUI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Devices_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ControllerDetails();
        }

        private void Profiles_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Profiles();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Settings();
        }

        protected void OnExit(object sender, EventArgs e)
        {
            InputDeviceManager.RemoveAllDevices();
            base.OnClosed(e);
        }
    }
}
