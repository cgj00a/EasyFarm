using EasyFarm.Classes;
using MemoryAPI;
using Ploeh.AutoFixture;
using Xunit;

namespace EasyFarm.Tests.Classes
{
    public class MobTest
    {
        [Fact]
        public void UnitInMeleeDistance()
        {
            // Fixture setup
            var playerTools = TestHelper.CreatePlayerTools();
            var npcTools = TestHelper.CreateNpcTools(Status.Standing);
            var partyMemberTools = TestHelper.CreatePartyMemberTools();
            var memory = TestHelper.CreateMemory(playerTools, npcTools, partyMemberTools);
            var mob = new Unit(memory, 0);

            // Exercise system
            var result = UnitFilters.MobFilter(memory, mob);

            // Verify outcome	
            Assert.True(result);

            // Teardown
        }

        public class IsAttackable
        {
            [Fact]
            public void MobIsAttackable()
            {
                // Fixture setup
                var fixture = new Fixture();

                var mob = new Mob(
                    isActive: true,
                    status: Status.Standing,
                    isRendered: true,
                    isPet: false);

                // Exercise system
                var result = mob.IsAttackable;

                // Verify outcome	
                Assert.True(result);

                // Teardown
            }
        }

        public class IsInteractable
        {
            [Theory]
            [InlineData(true, false)]
            [InlineData(false, true)]
            [InlineData(false, false)]
            public void WithNonInteractiveMobReturnsCorrectResult(bool isActive, bool isRendered)
            {
                // Fixture setup
                var mob = CreateMob(isActive, isRendered);

                // Exercise system
                var result = mob.IsInteractable;

                // Verify outcome	
                Assert.False(result);

                // Teardown
            }

            [Fact]
            public void WithInteractiveMobReturnsCorrectResult()
            {
                // Fixture setup
                var mob = CreateMob(isActive: true, isRendered: true);

                // Exercise system
                var result = mob.IsInteractable;

                // Verify outcome	
                Assert.True(result);

                // Teardown
            }

            private static Mob CreateMob(bool isActive, bool isRendered)
            {
                return new Mob(isActive, Status.Unknown, isRendered, false);
            }
        }

        public class IsDead
        {
            [Theory]
            [InlineData(Status.Dead1)]
            [InlineData(Status.Dead2)]
            public void WithDeadStatusReturnsCorrectResult(Status deadStatus)
            {
                // Fixture setup
                var mob = CreateMob(deadStatus);

                // Exercise system
                var result = mob.IsDead;

                // Verify outcome	
                Assert.True(result);
                // Teardown
            }

            private static Mob CreateMob(Status deadStatus)
            {
                return new Mob(false, deadStatus, false, false);
            }
        }
    }

    public class Mob
    {
        private readonly bool _isActive;
        private readonly Status _status;
        private readonly bool _isRendered;
        private readonly bool _isPet;

        public Mob(
            bool isActive,
            Status status,
            bool isRendered,
            bool isPet)
        {
            _isActive = isActive;
            _status = status;
            _isRendered = isRendered;
            _isPet = isPet;
        }

        public bool IsDead => _status == Status.Dead1 || _status == Status.Dead2;

        public bool IsAttackable
        {
            get
            {
                return _isActive && !IsDead && _isRendered && !_isPet;
            }
        }

        public bool IsInteractable => _isActive && _isRendered;
    }
}