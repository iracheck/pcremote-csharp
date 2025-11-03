using ControllerToMouse.Devices;
using System;
using System.Collections.Generic;
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

using ControllerToMouse.Source.GUI.UserControls;
using System.Windows.Threading;

namespace ControllerToMouse.Source.GUI.Submenus
{
    /// <summary>
    /// Interaction logic for DevicesMenu.xaml
    /// </summary>
    public partial class DevicesMenu : UserControl
    {
        public DispatcherTimer UpdateTimer = new DispatcherTimer();

        public DevicesMenu()
        {
            InitializeComponent();

            UpdateTimer.Interval = TimeSpan.FromMilliseconds(250);

            UpdateTimer.Tick += UpdateTimer_Tick;

            UpdateTimer.Start();

            LoadControllers();
            UpdateTimer_Tick(null, null); // do the first tick immediately, just to ensure display accuracy.
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            foreach (var controller in ControllerCardList.Children)
            {
                if (controller is ControllerCard card)
                {
                    card.Update();
                }
            }
        }

        public void LoadControllers()
        {
            ControllerCardList.Children.Clear();
            InputDeviceManager.InitializeDevices();

            for (int i = 0; i < 4; i++)
            {
                ControllerToMouse.Devices.InputDevice device = InputDeviceManager.GetDevice(i);

                if (device == null) continue;
                else
                {
                    ControllerCard card = new ControllerCard(device);

                    ControllerCardList.Children.Add(card);
                }
            }
        }

        public void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadControllers();
        }

        public void UpdateDevices()
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
