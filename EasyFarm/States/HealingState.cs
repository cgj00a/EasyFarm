/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI.>
Copyright (C) <2013>  <Zerolimits>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
*/
///////////////////////////////////////////////////////////////////

using System.Linq;
using EasyFarm.Classes;
using EliteMMO.API;
using MemoryAPI;

namespace EasyFarm.States
{
    public class HealingState : BaseState
    {
        private readonly Executor _executor;
        private readonly UnitService _unitService;

        public HealingState(IMemoryAPI fface) : base(fface)
        {
            _executor = new Executor(fface);
            _unitService = new UnitService(fface);
        }

        public override bool CheckComponent()
        {
            return !new RestState(fface).CheckComponent();
        }

        public override void EnterComponent()
        {
            // Stop resting. 
            if (fface.Player.Status.Equals(Status.Healing))
            {
                Player.Stand(fface);
            }

            // Stop moving. 
            fface.Navigator.Reset();
        }

        public override void RunComponent()
        {           
            foreach (var availableMove in Config.Instance.BattleLists["Healing"].Actions)
            {
                foreach (var partyMember in fface.PartyMember.Values)
                {
                    if (fface.Navigator.DistanceTo(partyMember.Position) <= availableMove.Distance)
                    {
                        var target = _unitService.PlayerArray.FirstOrDefault(x => x.Id == partyMember.ServerID);
                        if (target == null) return;

                        if (ActionFilters.TargetedFilter(fface, availableMove, target))
                        {                            
                            _executor.UseTargetedAction(availableMove, target);
                        }
                    }
                }
            }
        }
    }
}