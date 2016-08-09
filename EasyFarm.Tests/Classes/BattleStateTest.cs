using System;
using System.Collections.Generic;
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
                var memory = CreateMemory();
                var battleState = new BattleState(memory);
                CombatBaseState.IsFighting = false;

                // Exercise system
                var result = battleState.CheckComponent();

                // Verify outcome
                Assert.False(result);

                // Teardown
            }

            public INPCTools CreateNpcTools(Status status)
            {
                var npc = new Mock<INPCTools>();
                npc.Setup(x => x.IsActive(0)).Returns(true);
                npc.Setup(x => x.ClaimedID(0)).Returns(0);
                npc.Setup(x => x.HPPCurrent(0)).Returns(100);
                npc.Setup(x => x.IsClaimed(0)).Returns(false);
                npc.Setup(x => x.IsRendered(0)).Returns(true);
                npc.Setup(x => x.NPCType(0)).Returns(NpcType.Mob);
                npc.Setup(x => x.Name(0)).Returns("Mandragora");
                npc.Setup(x => x.Status(0)).Returns(status);
                return npc.Object;
            }

            [Fact]
            public void FightingWithEngagedNotSetReturnsCorrectResult()
            {
                // Fixture setup
                var player = new Mock<IPlayerTools>();
                player.Setup(x => x.Status).Returns(Status.Standing);

                var npc = CreateNpcTools(Status.Standing);

                var party = new Mock<IPartyMemberTools>();
                party.Setup(x => x.ServerID).Returns(0);

                var memory = CreateMemory(player.Object, npc, party.Object);
                var sut = CreateSut(memory);

                CombatBaseState.IsFighting = true;

                CombatBaseState.Target = new Unit(memory, 0);

                Config.Instance.IsEngageEnabled = true;

                // Exercise system
                var result = sut.CheckComponent();

                // Verify outcome
                Assert.False(result);

                // Teardown
            }

            [Fact]
            public void FightingWithEngagedSetReturnsCorrectResult()
            {
                // Fixture setup
                var player = new Mock<IPlayerTools>();
                player.Setup(x => x.Status).Returns(Status.Fighting);

                var npc = CreateNpcTools(Status.Fighting);

                var party = new Mock<IPartyMemberTools>();
                party.Setup(x => x.ServerID).Returns(0);

                var memory = CreateMemory(player.Object, npc, party.Object);
                var sut = CreateSut(memory);

                CombatBaseState.IsFighting = true;

                CombatBaseState.Target = new Unit(memory, 0);

                Config.Instance.IsEngageEnabled = true;

                // Exercise system
                var result = sut.CheckComponent();

                // Verify outcome
                Assert.True(result);

                // Teardown
            }            

            private static IMemoryAPI CreateMemory(IPlayerTools playerTools)
            {
                var memory = new Mock<IMemoryAPI>();
                memory.SetReturnsDefault(playerTools);
                return memory.Object;
            }

            private IMemoryAPI CreateMemory(
                IPlayerTools playerTools,
                INPCTools npcTools,
                IPartyMemberTools partyMemberTools)
            {
                var memory = new Mock<IMemoryAPI>();
                memory.SetReturnsDefault(playerTools);
                memory.SetReturnsDefault(npcTools);
                memory.SetReturnsDefault(partyMemberTools);
                memory.Setup(x => x.PartyMember).Returns(new Dictionary<byte, IPartyMemberTools> { { 0, partyMemberTools } });
                return memory.Object;
            }

            private BattleState CreateSut(IMemoryAPI memory)
            {
                return new BattleState(memory);
            }

            private static IMemoryAPI CreateMemory()
            {
                var memory = new Mock<IMemoryAPI>();
                memory.SetReturnsDefault(Mock.Of<IPlayerTools>());
                return memory.Object;
            }
        }
    }
}
