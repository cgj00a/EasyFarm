using System.Windows;
using EasyFarm.Logging;
using EasyFarm.Parsing;
using EasyFarm.ViewModels;
using EasyFarm.Views;

namespace EasyFarm.Infrastructure
{
    public class EasyFarmContainer : IEasyFarmContainer
    {
        public EasyFarmContainer()
        {            
        }

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