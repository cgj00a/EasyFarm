using System;
using System.Windows;
using System.Windows.Input;
using EasyFarm.Views;

namespace EasyFarm.ViewModels
{
    public class WindowAdapter : IWindow
    {
        private readonly Window _wpfWindow;

        public WindowAdapter(Window wpfWindow)
        {
            if (wpfWindow == null) throw new ArgumentNullException(nameof(wpfWindow));
            _wpfWindow = wpfWindow;
        }

        public void Close() => _wpfWindow.Close();

        public IWindow CreateChild(object viewModel)
        {
            var cw = new ContentWindow
            {
                Owner = _wpfWindow,
                DataContext = viewModel
            };

            ConfigureBehavior(cw);

            return new WindowAdapter(cw);
        }

        public void Show() => _wpfWindow.Show();

        public bool? ShowDialog() => _wpfWindow.ShowDialog();

        protected Window WpfWindow => _wpfWindow;

        private static void ConfigureBehavior(ContentWindow cw)
        {
            cw.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            cw.CommandBindings.Add(new CommandBinding(PresentationCommands.Accept, (sender, e) => cw.DialogResult = true));
        }
    }
}