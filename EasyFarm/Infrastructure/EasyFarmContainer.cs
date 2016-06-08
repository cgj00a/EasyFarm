using System.Windows;
using EasyFarm.ViewModels;
using EasyFarm.Views;

namespace EasyFarm.Infrastructure
{
    public class EasyFarmContainer : IEasyFarmContainer
    {
        public IWindow ResolveWindow()
        {            
            IMasterWindowViewModelFactory vmFactory = new MasterWindowViewModelFactory();
            Window mainWindow = new MasterView();
            IWindow w = new MasterWindowAdapter(mainWindow, vmFactory);
            return w;
        }
    }

    public interface IEasyFarmContainer
    {
    }
}