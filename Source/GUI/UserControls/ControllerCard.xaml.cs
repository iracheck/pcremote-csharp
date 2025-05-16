using ControllerToMouse.Devices;
using SharpDX.XInput;
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

namespace ControllerToMouse.GUI.UserControls
{
    /// <summary>
    /// Interaction logic for ControllerCard.xaml
    /// </summary>
    public partial class ControllerCard : UserControl
    {
        private UserIndex index;
        private ControllerToMouse.Devices.InputDevice device;

        public ControllerCard(int i)
        {
            index = (UserIndex)i;
            device = InputDeviceManager.GetDevice(i);

            InitializeComponent();
        }
    }
}
