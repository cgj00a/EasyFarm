using System;
using System.Windows;
using EasyFarm.ViewModels;

namespace EasyFarm.Infrastructure
{
    public class MasterWindowAdapter : WindowAdapter
    {
        private readonly IMasterWindowViewModelFactory _vmFactory;
        private bool _initialized;

        public MasterWindowAdapter(Window wpfWindow, IMasterWindowViewModelFactory viewModelFactory)
            : base(wpfWindow)
        {
            if (viewModelFactory == null) throw new ArgumentNullException(nameof(viewModelFactory));
            _vmFactory = viewModelFactory;
        }

        #region IWindow Members

        public override void Close()
        {
            EnsureInitialized();
            base.Close();
        }

        public override IWindow CreateChild(object viewModel)
        {
            EnsureInitialized();
            return base.CreateChild(viewModel);
        }

        public override void Show()
        {
            EnsureInitialized();
            base.Show();
        }

        public override bool? ShowDialog()
        {
            EnsureInitialized();
            return base.ShowDialog();
        }

        #endregion

        private void DeclareKeyBindings(MasterViewModel vm)
        {
            //WpfWindow.InputBindings.Add(new KeyBinding(vm.RefreshCommand, new KeyGesture(Key.F5)));
            //WpfWindow.InputBindings.Add(new KeyBinding(vm.InsertProductCommand, new KeyGesture(Key.Insert)));
            //WpfWindow.InputBindings.Add(new KeyBinding(vm.EditProductCommand, new KeyGesture(Key.Enter)));
            //WpfWindow.InputBindings.Add(new KeyBinding(vm.DeleteProductCommand, new KeyGesture(Key.Delete)));
        }

        private void EnsureInitialized()
        {
            if (_initialized) return;
            var vm = _vmFactory.Create(this);
            WpfWindow.DataContext = vm;
            DeclareKeyBindings(vm);
            _initialized = true;
        }
    }
}