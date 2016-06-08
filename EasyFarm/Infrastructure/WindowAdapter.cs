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

using System;
using System.Windows;
using System.Windows.Input;
using EasyFarm.ViewModels;
using EasyFarm.Views;

namespace EasyFarm.Infrastructure
{
    public class WindowAdapter : IWindow
    {
        private readonly Window _wpfWindow;

        protected WindowAdapter(Window wpfWindow)
        {
            if (wpfWindow == null)
            {
                throw new ArgumentNullException(nameof(wpfWindow));
            }

            _wpfWindow = wpfWindow;
        }

        #region IWindow Members

        public virtual void Close()
        {
            _wpfWindow.Close();
        }

        public virtual IWindow CreateChild(object viewModel)
        {
            var cw = new ContentWindow();
            cw.Owner = _wpfWindow;
            cw.DataContext = viewModel;
            ConfigureBehavior(cw);

            return new WindowAdapter(cw);
        }

        public virtual void Show()
        {
            _wpfWindow.Show();
        }

        public virtual bool? ShowDialog()
        {
            return _wpfWindow.ShowDialog();
        }

        #endregion

        protected Window WpfWindow => _wpfWindow;

        private static void ConfigureBehavior(ContentWindow cw)
        {
            cw.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            cw.CommandBindings.Add(new CommandBinding(PresentationCommands.Accept, (sender, e) => cw.DialogResult = true));
            cw.Height = cw.Owner.Height * .90;
            cw.Width = cw.Owner.Width * .90;
        }
    }
}
