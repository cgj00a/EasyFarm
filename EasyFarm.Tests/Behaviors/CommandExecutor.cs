using EliteMMO.API;

namespace EasyFarm.Tests.Behaviors
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly EliteAPI _eliteApi;

        public CommandExecutor(EliteAPI eliteApi)
        {
            _eliteApi = eliteApi;
        }

        public void Execute(string command)
        {
            _eliteApi.ThirdParty.SendString(command);
        }
    }
}