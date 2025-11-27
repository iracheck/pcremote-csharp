using ControllerToMouse.Devices;
using ControllerToMouse.Meta;
using ControllerToMouse.Settings;
using ControllerToMouse.Source.GUI;
using ControllerToMouse.Source.GUI.Submenus;
using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;



namespace ControllerToMouse.GUI
{
    public partial class MainWindow : Window
    {
        // Menus
        private readonly DevicesMenu DeviceMenu = new DevicesMenu();
        private readonly SettingsMenu SettingMenu = new SettingsMenu();
        private readonly ProfilesMenu ProfileMenu = new ProfilesMenu();

        public DispatcherTimer UpdateTimer = new DispatcherTimer();

        private List<Button> NavButtons;

        const float UPDATE_INTERVAL = 0.1f; // in seconds

        public MainWindow()
        {
            InitializeComponent();

            // initialize status bar information
            StatusBar.UpdateInterval = UPDATE_INTERVAL;
            StatusBar.Target = StatusMessage;

            // ensure file directories & load app settings. This will also create them and save default settings if they dont exist
            FilePaths.EnsureDirectoriesExist();
            AppSettings.Load();

            // Store nav buttons for later reference
            NavButtons = new List<Button>
            {
                DevicesTab,
                ProfilesTab,
                SettingsTab
            };

            // set default tab to DeviceMenu
            ContentArea.Content = DeviceMenu;

            // set version number to Meta.BuildInfo's stored value
            VersionNumber.Text = "v" + Meta.BuildInfo.Version;

            // Initialize the global timer that the program works with:
            UpdateTimer.Interval = TimeSpan.FromMilliseconds(100);

                // the timer handles multiple things: most important of which is to update all of the devices on demand
            UpdateTimer.Tick += DeviceMenu.UpdateTimer_Tick;

                // update the status box, too.
            UpdateTimer.Tick += StatusBar.Update;

            UpdateTimer.Start();

            StatusBar.Message("Test");
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

            if (AppSettings.AppBehavior.MinimizeToTray)
            {
                this.Hide();
            }
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
