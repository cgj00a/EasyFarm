using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace EasyFarm.Parsing
{
    public class ResourceLoader : IResourceLoader
    {
        private readonly string filePath;

        public ResourceLoader(string filePath)
        {
            this.filePath = filePath;
        }

        /// <summary>
        ///     Ensures that the resource file passed exists
        ///     and returns the XElement obj associated with the file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public IEnumerable<XElement> LoadResources()
        {
            // List to store all read resources.
            // Get a list of all resource file names.
            if (!Directory.Exists(filePath)) return new List<XElement>();
            var files = Directory.GetFiles(filePath, "*.xml");

            // Load all resource files in the given directory.
            return files.Select(XElement.Load).ToList();
        }        
    }
}