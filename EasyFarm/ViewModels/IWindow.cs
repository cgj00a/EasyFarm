namespace EasyFarm.ViewModels
{
    // http://stackoverflow.com/questions/2956027/how-to-build-a-generic-re-usable-modal-dialog-for-wpf-following-mvvm

    public interface IWindow
    {
        void Close();

        IWindow CreateChild(object viewModel);

        void Show();

        bool? ShowDialog();
    }
}