using System.IO;
using System;
using AudioReplacer2.Util;
using AudioReplacer2.Pages;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml.Media;
using WinRT.Interop;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using Microsoft.UI.Xaml;

namespace AudioReplacer2
{
    public sealed partial class MainWindow
    {
        private readonly Dictionary<Type, Page> pageCache = new();

        // Needed for button state switching. These are in MainWindow because checking for them is a part of the window shutting down (Okay this probably isn't the best way to do this but if it ain't broke, don't fix it)
        public static bool IsProcessing;
        public static bool IsRecording;
        public static bool ProjectInitialized;
        public static string CurrentFile;

        public MainWindow()
        {
            InitializeComponent();
            GlobalData.AppWindow = GetAppWindowForCurrentWindow(this);
            GlobalData.AppWindow.Closing += OnWindowClose;

            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);
            AppVersion.Text = GlobalData.GetAppVersion();

            // Set everything that can be set by the settings.
            if (Content is FrameworkElement rootElement) rootElement.RequestedTheme = (ElementTheme) App.AppSettings.AppThemeSetting;
            
            if (App.AppSettings.AppTransparencySetting == 0)
            {
                switch (MicaController.IsSupported())
                {
                    case true:
                        var micaBackdrop = new MicaBackdrop();
                        SystemBackdrop = micaBackdrop;
                        break;
                    case false:
                        var acrylicBackdrop = new DesktopAcrylicBackdrop();
                        SystemBackdrop = acrylicBackdrop;
                        break;
                }
            }
            else if (App.AppSettings.AppTransparencySetting == 1)
            {
                var acrylicBackdrop = new DesktopAcrylicBackdrop();
                SystemBackdrop = acrylicBackdrop;
            }
            else SystemBackdrop = null;

            // Finally, open the recording page
            ContentFrame.Navigate(typeof(RecordPage));
        }

        private AppWindow GetAppWindowForCurrentWindow(object window) // Thanks StackOverflow man!
        {
            var hWnd = WindowNative.GetWindowHandle(window);
            var currentWndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            return AppWindow.GetFromWindowId(currentWndId);
        }

        private void SwitchPageContent(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var pageSwitchType = typeof(RecordPage); // Default page of project
            if (args.InvokedItemContainer != null && args.InvokedItemContainer.Tag is string tag)
            {
                pageSwitchType = tag switch
                {
                    "Record" => typeof(RecordPage),
                    "Settings" => typeof(SettingsPage),
                    "Pitch Editor" => typeof(PitchDataEditor),
                    _ => pageSwitchType
                };
            }
            if (!pageCache.TryGetValue(pageSwitchType, out var page)) { page = (Page) Activator.CreateInstance(pageSwitchType); pageCache[pageSwitchType] = page; }
            ContentFrame.Content = page;
        }
        private void OnWindowClose(object sender, AppWindowClosingEventArgs args) { if (MainWindow.ProjectInitialized && (MainWindow.IsProcessing || MainWindow.IsRecording)) File.Delete(MainWindow.CurrentFile); }
    }
}
