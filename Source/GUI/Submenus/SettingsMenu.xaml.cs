using ControllerToMouse.Source.GUI.Submenus.Settings_Subpages;
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

namespace ControllerToMouse.Source.GUI.Submenus
{
    /// <summary>
    /// Interaction logic for SettingsMenu.xaml
    /// </summary>
    public partial class SettingsMenu : UserControl
    {
        GeneralSettings GeneralSettingsMenu = new();
        PowerSavingSettings PowerSavingSettingsMenu = new();
        DangerZoneSettings DangerZoneSettingMenu = new();
        ProgramInfo ProgramInfoMenu = new();

        List<Button> NavButtons;
        UserControl ContentArea;





        public SettingsMenu()
        {
            InitializeComponent();

            NavButtons = new List<Button>
            {
                PowerSavingButton,
                GeneralButton,
                DangerZoneButton,
                AboutButton
            };

            // initialize the content area
            ContentArea = SettingsPageControl;
            SettingsPageControl.Content = GeneralSettingsMenu;
            GeneralSettingsMenu.Tag = "Active";
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SetActiveTab(sender as Button);
            }
            catch { Trace.WriteLine(e.ToString()); }
        }

        protected void SetActiveTab(Button newTab)
        {
            Trace.WriteLine("Trying to switch tab");
            foreach (var b in NavButtons)
            {
                b.Tag = null;
            }

            newTab.Tag = "Active";

            switch (newTab.Name)
            {
                case ("GeneralButton"):
                    ContentArea.Content = GeneralSettingsMenu;
                    break;
                case ("PowerSavingButton"):
                    ContentArea.Content = PowerSavingSettingsMenu;
                    break;
                case ("DangerZoneButton"):
                    ContentArea.Content = DangerZoneSettingMenu;
                    break;
                case ("AboutButton"):
                    ContentArea.Content = ProgramInfoMenu;
                    break;
            }
        }
    }
}
