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

using EasyFarm.Classes;
using MemoryAPI;

namespace EasyFarm.States
{
    public class ZoneState : BaseState
    {
        private Zone _zone;

        public ZoneState(IMemoryAPI fface) : base(fface)
        {
            _zone = fface.Player.Zone;
        }

        public override bool CheckComponent()
        {
            var zone = fface.Player.Zone;
            return _zone != zone || fface.Player.Stats.Str == 0;
        }

        public override void RunComponent()
        {
            // Set new zone.
            _zone = fface.Player.Zone;

            // Stop program from running to next waypoint.
            fface.Navigator.Reset();

            // Stop the engine from running.
            AppServices.SendPauseEvent();
        }
    }
}
