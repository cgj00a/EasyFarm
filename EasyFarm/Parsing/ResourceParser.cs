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
    public class ResourceParser : IResourceParser
    {
        /// <summary>
        ///     A collection of resources to search for values.
        /// </summary>
        private readonly IEnumerable<XElement> _resources;

        /// <summary>
        ///     Retrieve all resources within the given directory.
        /// </summary>
        /// <param name="resourcesPath"></param>
        public ResourceParser(string resourcesPath)
        {
            // Read in all resources in the resourcePath.
            _resources = LoadResources(resourcesPath);
        }

        /// <summary>
        /// Retrieves the first resource with the given name. 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Resource Create(string name)
        {
            var resources = GetResourcesByName(name).ToList();
            return resources.FirstOrDefault() ?? new Resource();
        }

        /// <summary>
        ///     Returns a list of all resource with a specific name.
        /// </summary>
        /// <param name="name">Name of the action</param>
        /// <returns>A list of actions with that name</returns>
        public IEnumerable<Resource> GetResourcesByName(string name)
        {
            return ParseResources(name);
        }

        private T ToValue<T>(XElement element, string attributeName)
        {
            if (element.HasAttributes && element.Attribute(attributeName) != null)
            {
                var value = element.Attribute(attributeName).Value;
                return (T)Convert.ChangeType(value, typeof (T));
            }

            return default(T);
        }

        /// <summary>
        ///     A general method for loading abilites from the .xml files.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private IEnumerable<Resource> ParseResources(string name)
        {
            // Stores the created Resources with the given name.
            var resources = new List<Resource>();

            // Stores the XElement attributes that match the name.
            var elements = new List<XElement>();

            // Select all matching XElement objects.
            foreach (var resource in _resources)
            {
                elements.AddRange(resource.Elements()
                    .Attributes()
                    .Where(x => x.Name == "english" || x.Name == "japanese")
                    .Where(x => x.Value.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                    .Select(x => x.Parent));
            }

            // Start extracting values from XElement;s and augment our resource objects.
            foreach (var element in elements.Where(x => x.HasAttributes))
            {
                var resource = new Resource
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

                resource.AbilityType = ResourceHelper.ToAbilityType(resource.Prefix);
                resource.CategoryType = ResourceHelper.ToCategoryType(resource.Type);
                resource.ElementType = ResourceHelper.ToElementType(resource.Element);
                resource.SkillType = ResourceHelper.ToSkillType(resource.Skill);
                resource.TargetType = ResourceHelper.ToTargetTypeFlags(resource.Targets);

                resources.Add(resource);
            }

            return resources;
        }

        /// <summary>
        ///     Ensures that the resource file passed exists
        ///     and returns the XElement obj associated with the file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private IEnumerable<XElement> LoadResources(string path)
        {
            // List to store all read resources.
            // Get a list of all resource file names.
            if (!Directory.Exists(path)) return new List<XElement>();
            var resources = Directory.GetFiles(path, "*.xml");

            // Load all resource files in the given directory.
            return resources.Select(XElement.Load).ToList();
        }
    }
}