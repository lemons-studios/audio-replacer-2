﻿using Microsoft.UI.Xaml;

namespace AudioReplacer2
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            mainWindow = new MainWindow();
            mainWindow.Activate();
        }

        private Window mainWindow;
    }
}
