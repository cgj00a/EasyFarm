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
using Prism.Mvvm;

namespace EasyFarm.ViewModels
{
    public class FollowViewModel : BindableBase
    {
        public string ViewName => "Follow";

        public string Name
        {
            get { return Config.Instance.FollowedPlayer; }
            set
            {
                Config.Instance.FollowedPlayer = value;
                AppServices.InformUser("Now following {0}.", value);
            }
        }

        public double Distance
        {
            get { return Config.Instance.FollowDistance; }
            set
            {
                SetProperty(ref Config.Instance.FollowDistance, value);
                AppServices.InformUser(string.Format("Follow Distance: {0}.", value));
            }
        }
    }
}
