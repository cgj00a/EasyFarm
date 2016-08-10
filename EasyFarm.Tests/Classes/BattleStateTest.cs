using EasyFarm.Classes;
using EasyFarm.States;
using MemoryAPI;
using Moq;
using Xunit;

namespace EasyFarm.Tests.Classes
{
    public class BattleStateTest
    {
        public class CheckComponent
        {

            [Fact]
            public void NotFightingReturnsCorrectResult()
            {
                // Fixture setup
                var memory = TestHelper.CreateMemory();
                var battleState = new BattleState(memory);
                CombatBaseState.IsFighting = false;

                // Exercise system
                var result = battleState.CheckComponent();

                // Verify outcome
                Assert.False(result);

                // Teardown
            }           

            [Theory]
            [InlineData(Status.Standing, false)]
            [InlineData(Status.Fighting, true)]
            public void FightingWithEngagedSetReturnsCorrectResult(Status status, bool expected)
            {
                // Fixture setup

                var playerTools = TestHelper.CreatePlayerTools(status);
                var npcTools = TestHelper.CreateNpcTools(Status.Standing);
                var partyMemberTools = TestHelper.CreatePartyMemberTools();
                var memory = TestHelper.CreateMemory(playerTools, npcTools, partyMemberTools);
                var sut = CreateSut(memory);

                CombatBaseState.IsFighting = true;

                CombatBaseState.Target = new Unit(memory, 0);

                Config.Instance.IsEngageEnabled = true;

                // Exercise system
                var actual = sut.CheckComponent();

                // Verify outcome
                Assert.Equal(expected, actual);

                // Teardown
            }                        

            private BattleState CreateSut(IMemoryAPI memory)
            {
                return new BattleState(memory);
            }
        }
    }
}
