/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI>
Copyright (C) Mykezero

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
///////////////////////////////////////////////////////////////////*/

using System.Windows;
using EasyFarm.Infrastructure;
using EasyFarm.Logging;
using EasyFarm.Properties;

namespace EasyFarm
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
            Current.DispatcherUnhandledException += (sender, e) =>
            {
                MessageBox.Show(e.Exception.Message, "An exception has occurred. ", MessageBoxButton.OK, MessageBoxImage.Error);
            };
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            EasyFarmContainer container = new EasyFarmContainer();
            container.ResolveWindow().Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Log.Write("Application exiting");
            Settings.Default.Save();
        }
    }
}