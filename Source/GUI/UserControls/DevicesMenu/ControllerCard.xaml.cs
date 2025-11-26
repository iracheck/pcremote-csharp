using ControllerToMouse.Devices;
using ControllerToMouse.Source.GUI.Submenus;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using InputDevice = ControllerToMouse.Devices.InputDevice;

namespace ControllerToMouse.Source.GUI.UserControls
{
    /// <summary>
    /// Interaction logic for ControllerCard.xaml
    /// </summary>
    public partial class ControllerCard : UserControl
    {
        InputDevice Device;

        public ControllerCard(InputDevice device)
        {
            InitializeComponent();

            Device = device;
            this.ControllerName.Text = device.Name;
        }

        public void Update()
        {
            UpdateConnectionStatus();
            UpdateBatteryStatus();
            UpdateProfileStatus();
         
        }

        public void UpdateConnectionStatus()
        {
            if (Device.GetIsAsleep())
            {
                ConnectionStatusColor.Fill = new SolidColorBrush(Colors.Gold);
                ConnectionStatusText.Text = "Sleeping";
            }
            else if (Device.GetIsActive())
            {
                ConnectionStatusColor.Fill = new SolidColorBrush(Colors.Green);
                ConnectionStatusText.Text = "Active";
            }
            else
            {
                ConnectionStatusColor.Fill = new SolidColorBrush(Colors.Green);
                ConnectionStatusText.Text = "Connected";
            }
            
        }

        public void UpdateBatteryStatus()
        {
            
            if (Device.IsBatteryPowered)
            {
                BatteryLevelDisplay.Text = Device.BatteryLevel.ToString() + "%";
            }
            else
            {
                BatteryLevelDisplay.Text = "Wired";
            }

        }

        public void UpdateProfileStatus()
        {
            CurrentProfileDisplay.Text = Device.ProfileName;

        }
    }
}
