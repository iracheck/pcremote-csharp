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
        // Quick Actions
        List<ctmAction> Actions = new List<ctmAction>()
        { 
            new ReloadControllersAction(),
            new VibrationTest()
        };

        public DevicesMenu()
        {
            InitializeComponent();

            LoadControllers();
            LoadActions();

            UpdateTimer_Tick(null, null); // do the first update tick on the first frame, to ensure information accuracy
        }

        public void UpdateTimer_Tick(object sender, EventArgs e)
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

        public void LoadActions()
        {
            ActionList.Children.Clear();

            foreach (var action in Actions)
            {
                QuickAction card = new QuickAction(action);

                card.ActionName.Text = action.Name;
                card.ActionDescription.Text = action.Description;

                ActionList.Children.Add(card);
            }
        }

        public void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadControllers();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
