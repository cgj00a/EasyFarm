namespace EasyFarm.Tests.Behaviors
{
    public class SendCommandBehavior
    {
        private readonly ICommandExecutor _commandExecutor;

        public SendCommandBehavior(ICommandExecutor commandExecutor)
        {
            _commandExecutor = commandExecutor;
        }

        public BehaviorStatus Status { get; private set; }

        internal void Execute(string command)
        {
            _commandExecutor.Execute(command);
            Status = BehaviorStatus.Success;
        }
    }
}