using System.Collections.Generic;
using MemoryAPI;
using Moq;

namespace EasyFarm.Tests.Classes
{
    public class TestHelper
    {
        public static IPartyMemberTools CreatePartyMemberTools()
        {
            var party = new Mock<IPartyMemberTools>();
            party.Setup(x => x.ServerID).Returns(0);
            return party.Object;
        }

        public static INPCTools CreateNpcTools(Status status)
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

        public static IPlayerTools CreatePlayerTools(Status status = Status.Unknown)
        {
            var player = new Mock<IPlayerTools>();
            player.Setup(x => x.Status).Returns(status);
            return player.Object;
        }

        public static IMemoryAPI CreateMemory(
            IPlayerTools playerTools = null,
            INPCTools npcTools = null,
            IPartyMemberTools partyMemberTools = null)
        {
            var memory = new Mock<IMemoryAPI>();
            memory.SetReturnsDefault(playerTools ?? Mock.Of<IPlayerTools>());
            memory.SetReturnsDefault(npcTools ?? Mock.Of<INPCTools>());
            memory.SetReturnsDefault(partyMemberTools ?? Mock.Of<IPartyMemberTools>());
            memory.Setup(x => x.PartyMember).Returns(new Dictionary<byte, IPartyMemberTools> { { 0, partyMemberTools ?? Mock.Of<IPartyMemberTools>() } });
            return memory.Object;
        }
    }
}