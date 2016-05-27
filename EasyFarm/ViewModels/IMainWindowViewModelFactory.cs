namespace EasyFarm.ViewModels
{
    public interface IMainWindowViewModelFactory
    {
        MainWindowViewModel Create(IWindow window);
    }
}