using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace EasyFarm.Tests.Behaviors
{
    public class SendCommandBehaviorTest
    {
        [Theory, AutoMoqData]
        public void SendCommandReturnsCorrectResult(SendCommandBehavior sut)
        {
            // Fixture setup
            // Exercise system
            sut.Execute("/ta <t>");
            // Verify outcome            
            Assert.Equal(BehaviorStatus.Success, sut.Status);
            // Teardown
        }
    }

    public class CastSpellBehaviorTest
    {
        [Theory, AutoMoqData]
        public void CastSpellShouldReturnRunningWhileCasting(CastSpellBehavior sut)
        {
            // Fixture setup
            var world = new World
            {
                CastPercent = 10,
                IsCasting = true
            };
            // Exercise system
            sut.Execute(@"/ma ""Cure"" <me>", world);
            // Verify outcome
            Assert.Equal(BehaviorStatus.Running, sut.Status);
            // Teardown
        }
    }

    public class World
    {
        public bool IsCasting { get; set; }

        public int CastPercent { get; set; }
    }

    public class CastSpellBehavior
    {
        public BehaviorStatus Status { get; private set; }

        public void Execute(string command, World world)
        {
            if (world.IsCasting && world.CastPercent < 100) Status = BehaviorStatus.Running;
        }
    }
}
