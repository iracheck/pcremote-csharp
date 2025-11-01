using ControllerToMouse.Devices;
using ControllerToMouse.Source.GUI.Submenus;
using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
namespace ControllerToMouse.GUI
{
    public partial class MainWindow : Window
    {
        // Menus
        private readonly UserControl DeviceMenu = new DevicesMenu();
        private readonly UserControl SettingMenu = new SettingsMenu();
        private readonly UserControl ProfileMenu = new ProfilesMenu();
        private List<Button> NavButtons;

        public MainWindow()
        {
            Trace.WriteLine("Init main window");

            InitializeComponent();

            NavButtons = new List<Button>
            {
                DevicesTab,
                ProfilesTab,
                SettingsTab
            };

            ContentArea.Content = DeviceMenu;

        }

        protected void MenuButtonClick(object sender, EventArgs e)
        {
            try { 
                SetActiveTab(sender as Button);
            } 
            catch { Debug.WriteLine("Tried to trigger a menu button, but the triggered object wasnt a button! "); }
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
                case ("DevicesTab"):
                    ContentArea.Content = DeviceMenu;
                    break;
                case ("ProfilesTab"):
                    ContentArea.Content = ProfileMenu;
                    break;
                case ("SettingsTab"):
                    ContentArea.Content = SettingMenu;
                    break;
            }
        }


        // Header
        protected void HeaderMouseButton_Down(object sender, EventArgs e)
        {
            DragMove();
        }

        protected void MinimizeButton_Click(object sender, EventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        protected void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }


        // Generic Listeners
        protected void OnExit(object sender, EventArgs e)
        {
            InputDeviceManager.RemoveAllDevices();
            base.OnClosed(e);
        }

    }
}
