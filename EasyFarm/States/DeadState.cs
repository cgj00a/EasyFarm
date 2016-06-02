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
    public class DeadState : BaseState
    {
        public DeadState(IMemoryAPI fface) : base(fface) { }

        public override bool CheckComponent()
        {
            var status = fface.Player.Status;
            return status == Status.Dead1 || status == Status.Dead2;
        }

        public override void RunComponent()
        {
            // Stop program from running to next waypoint.
            fface.Navigator.Reset();

            // Stop the engine from running.
            AppServices.SendPauseEvent();
        }
    }
}
