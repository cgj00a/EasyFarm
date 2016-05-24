using System;
using System.Collections.Generic;
using System.Linq;
using EasyFarm;
using EliteMMO.API;

namespace MemoryAPI.Memory
{
    public class EliteMmoWrapper : MemoryWrapper
    {
        public enum ViewMode
        {
            ThirdPerson = 0,
            FirstPerson
        }

        public EliteMmoWrapper(int pid)
        {
            var eliteApi = new EliteAPI(pid);
            Navigator = new NavigationTools(eliteApi);
            NPC = new NpcTools(eliteApi);
            PartyMember = new Dictionary<byte, IPartyMemberTools>();
            Player = new PlayerTools(eliteApi);
            Target = new TargetTools(eliteApi);
            Timer = new TimerTools(eliteApi);
            Windower = new WindowerTools(eliteApi);

            for (byte i = 0; i < 16; i++)
            {
                PartyMember.Add(i, new PartyMemberTools(eliteApi, i));
            }
        }

        public class NavigationTools : INavigatorTools
        {
            private readonly EliteAPI _api;

            public double DistanceTolerance { get; set; } = 3;

            public NavigationTools(EliteAPI api)
            {
                _api = api;
            }

            /// <summary>
            /// Makes the player look at the specified position. 
            /// </summary>            
            /// Author: SMD111
            /// https://github.com/smd111/EliteMMO.Scripted
            public bool FaceHeading(IPosition position)
            {
                var player = _api.Entity.GetLocalPlayer();
                var angle = (byte)(Math.Atan((position.Z - player.Z) / (position.X - player.X)) * -(128.0f / Math.PI));
                if (player.X > position.X) angle += 128;
                var radian = (((float)angle) / 255) * 2 * Math.PI;
                return _api.Entity.SetEntityHPosition(_api.Entity.LocalPlayerIndex, (float)radian);
            }

            public double DistanceTo(IPosition position)
            {
                var player = _api.Entity.GetLocalPlayer();

                return Math.Sqrt(
                    Math.Pow(position.X - player.X, 2) +
                    Math.Pow(position.Y - player.Y, 2) +
                    Math.Pow(position.Z - player.Z, 2));
            }

            public void Goto(IPosition position, bool keepRunning)
            {
                if (DistanceTo(position) > DistanceTolerance)
                {
                    DateTime duration = DateTime.Now.AddSeconds(5);
                    _api.ThirdParty.KeyDown(Keys.NUMPAD8);

                    while (DistanceTo(position) > DistanceTolerance && DateTime.Now < duration)
                    {
                        if ((ViewMode)_api.Player.ViewMode != ViewMode.FirstPerson)
                        {
                            _api.Player.ViewMode = (int)ViewMode.FirstPerson;
                        }

                        FaceHeading(position);

                        System.Threading.Thread.Sleep(30);
                    }

                    _api.ThirdParty.KeyUp(Keys.NUMPAD8);
                }
            }

            public void GotoNPC(int id)
            {
                var entity = _api.Entity.GetEntity(id);
                Goto(Helpers.ToPosition(entity.X, entity.Y, entity.Z, entity.H), false);
            }

            public void Reset()
            {
                _api.ThirdParty.KeyUp(Keys.NUMPAD8);
            }
        }

        public class NpcTools : INPCTools
        {
            private readonly EliteAPI _api;

            public NpcTools(EliteAPI api)
            {
                _api = api;
            }

            public int ClaimedID(int id) { return (int)_api.Entity.GetEntity(id).ClaimID; }

            public double Distance(int id) { return _api.Entity.GetEntity(id).Distance; }

            public IPosition GetPosition(int id)
            {
                var entity = _api.Entity.GetEntity(id);
                return Helpers.ToPosition(entity.X, entity.Y, entity.Z, entity.H);
            }

            public short HPPCurrent(int id) { return _api.Entity.GetEntity(id).HealthPercent; }

            public bool IsActive(int id) { return true; }

            public bool IsClaimed(int id) { return _api.Entity.GetEntity(id).ClaimID != 0; }

            public int PetID(int id) => _api.Entity.GetEntity(id).PetIndex;

            /// <summary>
            /// Checks to see if the object is rendered. 
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            /// Author: SG1234567
            /// https://github.com/SG1234567
            public bool IsRendered(int id)
            {
                return (_api.Entity.GetEntity(id).Render0000 & 0x200) == 0x200;
            }

            public string Name(int id) { return _api.Entity.GetEntity(id).Name; }

            public NpcType NPCType(int id)
            {
                var entity = _api.Entity.GetEntity(id);
                if (entity.WarpPointer == 0) return NpcType.InanimateObject;
                if (IsOfType(entity.SpawnFlags, (int)NpcType.Mob)) return NpcType.Mob;
                if (IsOfType(entity.SpawnFlags, (int)NpcType.NPC)) return NpcType.NPC;
                if (IsOfType(entity.SpawnFlags, (int)NpcType.PC)) return NpcType.PC;
                if (IsOfType(entity.SpawnFlags, (int)NpcType.Self)) return NpcType.Self;
                return NpcType.InanimateObject;
            }

            private bool IsOfType(int one, int other)
            {
                return (one & other) == other;
            }

            public float PosX(int id) { return _api.Entity.GetEntity(id).X; }

            public float PosY(int id) { return _api.Entity.GetEntity(id).Y; }

            public float PosZ(int id) { return _api.Entity.GetEntity(id).Z; }

            public Status Status(int id)
            {
                var status = (EntityStatus)_api.Entity.GetEntity(id).Status;
                return Helpers.ToStatus(status);
            }
        }

        public class PartyMemberTools : IPartyMemberTools
        {
            private readonly EliteAPI _api;
            private readonly int _index;

            public PartyMemberTools(EliteAPI api, int index)
            {
                _api = api;
                _index = index;
            }

            private EliteAPI.PartyMember PartyMember => _api.Party.GetPartyMember(_index);

            private EliteAPI.XiEntity XiEntity => _api.Entity.GetEntity((int)PartyMember.ID);

            public int ServerID => (int)PartyMember.ID;

            public int HPPCurrent => PartyMember.CurrentHPP;

            public IPosition Position => Helpers.ToPosition(XiEntity.X, XiEntity.Y, XiEntity.Z, XiEntity.H);
        }

        public class PlayerTools : IPlayerTools
        {
            private readonly EliteAPI _api;

            public PlayerTools(EliteAPI api)
            {
                _api = api;
            }

            public float CastPercentEx => _api.CastBar.Percent * 100;

            public int HPPCurrent => (int)_api.Player.HPP;

            public int ID => _api.Player.ServerId;

            public int MPCurrent => (int)_api.Player.MP;

            public int MPPCurrent => (int)_api.Player.MPP;

            public string Name => _api.Player.Name;

            public IPosition Position
            {
                get
                {
                    var x = _api.Player.X;
                    var y = _api.Player.Y;
                    var z = _api.Player.Z;
                    var h = _api.Player.H;

                    return Helpers.ToPosition(x, y, z, h);
                }
            }

            public float PosX => Position.X;

            public float PosY => Position.Y;

            public float PosZ => Position.Z;

            public Structures.PlayerStats Stats
            {
                get
                {
                    var stats = _api.Player.Stats;

                    return new Structures.PlayerStats()
                    {
                        Agi = stats.Agility,
                        Chr = stats.Charisma,
                        Dex = stats.Dexterity,
                        Int = stats.Intelligence,
                        Mnd = stats.Mind,
                        Str = stats.Strength,
                        Vit = stats.Vitality
                    };
                }
            }

            public Status Status => Helpers.ToStatus((EntityStatus)_api.Player.Status);

            public StatusEffect[] StatusEffects
            {
                get
                {
                    return _api.Player.Buffs.Select(x => (StatusEffect)x).ToArray();
                }
            }

            public int TPCurrent => (int)_api.Player.TP;

            public Zone Zone => (Zone)_api.Player.ZoneId;
        }

        public class TargetTools : ITargetTools
        {
            private readonly EliteAPI _api;

            public TargetTools(EliteAPI api)
            {
                _api = api;
            }

            public int ID => (int)_api.Target.GetTargetInfo().TargetIndex;

            public bool SetNPCTarget(int index)
            {
                return _api.Target.SetTarget(index);
            }
        }

        public class TimerTools : ITimerTools
        {
            private readonly EliteAPI _api;

            public TimerTools(EliteAPI api)
            {
                _api = api;
            }

            public int GetAbilityRecast(int index)
            {
                var ids = _api.Recast.GetAbilityIds();
                var ability = _api.Resources.GetAbility((uint)index);
                var idx = ids.IndexOf(ability.TimerID);
                return _api.Recast.GetAbilityRecast(idx);
            }

            public int GetSpellRecast(int index)
            {
                return _api.Recast.GetSpellRecast(index);
            }
        }

        public class WindowerTools : IWindowerTools
        {
            private readonly EliteAPI _api;

            public WindowerTools(EliteAPI api)
            {
                _api = api;
            }

            public void SendString(string stringToSend)
            {
                _api.ThirdParty.SendString(stringToSend);
            }
        }
    }
}
