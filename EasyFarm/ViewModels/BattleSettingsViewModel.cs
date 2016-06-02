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

using EasyFarm.Classes;
using EasyFarm.Infrastructure;

namespace EasyFarm.ViewModels
{
    public class BattleSettingsViewModel : ViewModelBase
    {
        public bool ShouldEngage
        {
            get { return Config.Instance.IsEngageEnabled; }
            set { SetProperty(ref Config.Instance.IsEngageEnabled, value); }
        }

        public bool ShouldApproach
        {
            get { return Config.Instance.IsApproachEnabled; }
            set { SetProperty(ref Config.Instance.IsApproachEnabled, value); }
        }
    }
}