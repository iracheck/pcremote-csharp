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
    /// Interaction logic for QuickAction.xaml
    /// </summary>
    public partial class QuickAction : UserControl
    {
        String Name;
        String Description;
        ctmAction ClickEvent;

        public QuickAction(ctmAction clickEvent)
        {
            InitializeComponent();
            this.ClickEvent = clickEvent;
        }

        public void OnClick(object sender, RoutedEventArgs e)
        {
            ClickEvent.Activate();
        }
    }
}
