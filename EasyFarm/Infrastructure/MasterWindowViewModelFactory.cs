using System;
using EasyFarm.ViewModels;

namespace EasyFarm.Infrastructure
{
    public class MasterWindowViewModelFactory : IMasterWindowViewModelFactory
    {
        public MasterViewModel Create(IWindow window)
        {
            if (window == null)
            {
                throw new ArgumentNullException(nameof(window));
            }

            return new MasterViewModel(window);
        }
    }
}
