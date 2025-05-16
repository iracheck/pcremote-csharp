using System.Windows;
using System.Diagnostics;

using ControllerToMouse.Devices;
using SharpDX.XInput;
using ControllerToMouse.GUI.UserControls;
namespace ControllerToMouse.GUI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Initializes any active controllers
            ReloadDevices();
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

        private void ReloadDevices()
        {
            InputDeviceManager.InitializeDevices();
        }

        protected void OnExit(ExitEventArgs e)
        {
            InputDeviceManager.RemoveAllDevices();
            base.OnClosed(e);
        }
    }
}
