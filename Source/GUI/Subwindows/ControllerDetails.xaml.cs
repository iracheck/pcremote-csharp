using ControllerToMouse.Devices;
using ControllerToMouse.GUI.UserControls;
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

namespace ControllerToMouse.GUI
{
    /// <summary>
    /// Interaction logic for ControllerDetails.xaml
    /// </summary>
    public partial class ControllerDetails : UserControl
    {
        public ControllerDetails()
        {
            InitializeComponent();

            ControllersPanel.Children.Clear();

            for (int i = 0; i < InputDeviceManager.MAX_CONTROLLERS; i++)
            {
                Debug.WriteLine("Adding Controller");
                var device = InputDeviceManager.GetDevice(i);
                var card = new ControllerCard(i);

                if (device != null)
                {
                    card.ControllerNameText.Text = device.Name;
                    card.ControllerProfileText.Text = device.ProfileName;
                }
                else
                {
                    card.ControllerNameText.Text = "No Controller Connected";
                    card.ControllerProfileText.Text = "";
                }

                ControllersPanel.Children.Add(card);
            }
        }
    }
}
