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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace EasyFarm.Parsing
{
    /// <summary>
    ///     A class for loading the ability and spell xmls from file.
    /// </summary>
    public class AbilityService
    {
        /// <summary>
        ///     A collection of resources to search for values.
        /// </summary>
        private readonly IEnumerable<XElement> resources;

        /// <summary>
        ///     Retrieve all resources within the given directory.
        /// </summary>
        /// <param name="resourceLoader"></param>
        public AbilityService(IResourceLoader resourceLoader)
        {
            // Read in all resources in the resourcePath.
            resources = resourceLoader.LoadResources();
        }

        /// <summary>
        ///     Creates an ability obj. This object may be a spell
        ///     or an ability.
        /// </summary>
        /// <param name="name">Ability's Name</param>
        /// <returns>a new ability</returns>
        public Ability CreateAbility(string name)
        {
            var abilities = GetAbilitiesWithName(name);
            var enumerable = abilities as Ability[] ?? abilities.ToArray();
            if (!enumerable.Any()) return new Ability();
            return enumerable.First();
        }

        /// <summary>
        ///     Returns a list of all abilities with a specific name.
        /// </summary>
        /// <param name="name">Name of the action</param>
        /// <returns>A list of actions with that name</returns>
        public IEnumerable<Ability> GetAbilitiesWithName(string name)
        {
            return ParseResources(name);
        }

        private T ToValue<T>(XElement element, string attributeName)
        {
            if (!element.HasAttributes || element.Attribute(attributeName) == null) return default(T);
            var value = element.Attribute(attributeName).Value;
            return (T)Convert.ChangeType(value, typeof (T));
        }

        /// <summary>
        ///     A general method for loading abilites from the .xml files.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private IEnumerable<Ability> ParseResources(string name)
        {
            // Stores the created abilities with the given name.
            var abilities = new List<Ability>();

            // Stores the XElement attributes that match the name.
            var elements = new List<XElement>();

            // Select all matching XElement objects.
            foreach (var resource in resources)
            {
                elements.AddRange(resource.Elements()
                    .Attributes()
                    .Where(x => x.Name == "english" || x.Name == "japanese")
                    .Where(x => x.Value.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                    .Select(x => x.Parent));
            }

            // Start extracting values from XElement;s and augment our ability objects.
            foreach (var element in elements.Where(x => x.HasAttributes))
            {
                var ability = new Ability
                {
                    Alias = ToValue<string>(element, "alias"),
                    Element = ToValue<string>(element, "element"),
                    English = ToValue<string>(element, "english"),
                    Japanese = ToValue<string>(element, "japanese"),
                    Prefix = ToValue<string>(element, "prefix"),
                    Skill = ToValue<string>(element, "skill"),
                    Targets = ToValue<string>(element, "targets"),
                    Type = ToValue<string>(element, "type"),
                    CastTime = ToValue<double>(element, "casttime"),
                    Recast = ToValue<double>(element, "recast"),
                    Id = ToValue<int>(element, "id"),
                    Index = ToValue<int>(element, "index"),
                    MpCost = ToValue<int>(element, "mpcost"),
                    TpCost = ToValue<int>(element, "tpcost")
                };

                ability.AbilityType = ResourceHelper.ToAbilityType(ability.Prefix);
                ability.CategoryType = ResourceHelper.ToCategoryType(ability.Type);
                ability.ElementType = ResourceHelper.ToElementType(ability.Element);
                ability.SkillType = ResourceHelper.ToSkillType(ability.Skill);
                ability.TargetType = ResourceHelper.ToTargetTypeFlags(ability.Targets);

                abilities.Add(ability);
            }

            return abilities;
        }


    }
}