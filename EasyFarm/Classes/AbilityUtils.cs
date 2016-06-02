/*///////////////////////////////////////////////////////////////////
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

using EasyFarm.Parsing;
using MemoryAPI;

namespace EasyFarm.Classes
{
    public static class AbilityUtils
    {
        /// <summary>
        ///     Checks whether a spell or ability can be recasted.
        /// </summary>
        /// <param name="fface"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        public static bool IsRecastable(IMemoryAPI fface, Resource resource)
        {
            var recast = 0;

            // No recast time on weaponskills. 
            if (resource.AbilityType == AbilityType.Weaponskill) return true;

            // No recast for ranged attacks. 
            if (AbilityType.Range.HasFlag(resource.AbilityType)) return true;

            // If a spell get spell recast
            if (ResourceHelper.IsSpell(resource.AbilityType))
            {
                recast = fface.Timer.GetSpellRecast(resource.Index);
            }

            // if ability get ability recast. 
            if (ResourceHelper.IsAbility(resource.AbilityType))
            {
                recast = fface.Timer.GetAbilityRecast(resource.Id);
            }

            return recast <= 0;
        }

    }
}