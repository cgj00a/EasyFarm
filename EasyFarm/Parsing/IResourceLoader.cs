using System.Collections.Generic;
using System.Xml.Linq;

namespace EasyFarm.Parsing
{
    public interface IResourceLoader
    {
        IEnumerable<XElement> LoadResources();
    }
}