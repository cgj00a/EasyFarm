﻿/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI>
Copyright (C) Mykezero

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
///////////////////////////////////////////////////////////////////*/

using System.Linq;
using EasyFarm.Classes;
using MemoryAPI;

namespace EasyFarm.States
{
    /// <summary>
    ///     Handles the end of battle situation.
    ///     Fires off the end list, sets FightStart to true so other
    ///     lists can fire and replaces targets that are dead, null,
    ///     empty or invalid.
    /// </summary>
    public class EndState : CombatBaseState
    {
        private readonly Executor _executor;

        public EndState(IMemoryAPI fface) : base(fface)
        {
            _executor = new Executor(fface);
        }

        public override bool CheckComponent()
        {
            // Prevent making the player stand up from resting.
            if (new RestState(fface).CheckComponent()) return false;

            // Creature is unkillable and does not meets the
            // user's criteria for valid mobs defined in MobFilters.
            return !UnitFilters.MobFilter(fface, Target);
        }

        /// <summary>
        ///     Force player when changing targets.
        /// </summary>
        public override void EnterComponent()
        {
            fface.Navigator.Reset();

            while (fface.Player.Status == Status.Fighting)
            {
                Player.Disengage(fface);
            }
        }

        public override void RunComponent()
        {
            // Execute moves.
            var usable = Config.Instance.BattleLists["End"].Actions
                .Where(x => ActionFilters.BuffingFilter(fface, x));

            _executor.UseBuffingActions(usable);

            // Reset all usage data to begin a new battle.
            foreach (var action in Config.Instance.BattleLists.Actions)
            {
                action.Usages = 0;
            }
        }
    }
}