using ControllerToMouse.Settings;
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

namespace ControllerToMouse.Source.GUI.Submenus.Settings_Subpages
{
    /// <summary>
    /// Interaction logic for General.xaml
    /// </summary>
    public partial class GeneralSettings : UserControl
    {
        public GeneralSettings()
        {
            InitializeComponent();
        }

        private void VolumeStepSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            AppSettings.Audio.AudioStep = (int)VolumeStepSlider.Value;
        }

        private void CheckForUpdatesCheckbox_Changed(object sender, RoutedEventArgs e)
        {
            AppSettings.AppBehavior.CheckForUpdates = (bool)CheckForUpdatesCheckbox.IsChecked;
        }

        private void MinimizeCheckbox_Changed(object sender, RoutedEventArgs e)
        {
            AppSettings.AppBehavior.MinimizeToTray = (bool)MinimizeCheckbox.IsChecked;
        }

        private void StartUpCheckbox_Changed(object sender, RoutedEventArgs e)
        {
            AppSettings.AppBehavior.OpenOnStartup = (bool)CheckForUpdatesCheckbox.IsChecked;
        }

        private void ThemeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selection = ThemeBox.SelectedValue.ToString().ToLower();
            AppSettings.GUI.Theme = selection;

            // will have an actual functionality later
        }
    }
}
