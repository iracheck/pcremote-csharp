using System.Windows;
using System.Diagnostics;

using ControllerToMouse.Devices;
using SharpDX.XInput;
namespace ControllerToMouse.GUI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Debug.WriteLine("MainWindow loaded.");
        }

        private void StartController_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Starting controller service...");
            InputDeviceManager.InitializeDevices();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
