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

namespace ControllerToMouse.Source.GUI.Submenus.Settings_Subpages
{
    /// <summary>
    /// Interaction logic for ProgramInfo.xaml
    /// </summary>
    public partial class ProgramInfo : UserControl
    {
        public ProgramInfo()
        {
            InitializeComponent();

            CurrentVersionBox.Text = "Current Version: v" + Meta.BuildInfo.Version;
            BuildDateBox.Text = "Built on " + Meta.BuildInfo.BuildDate;
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }
    }
}
