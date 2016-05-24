using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using EasyFarm.Parsing;
using EasyFarm.Tests.Utilities;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace EasyFarm.Tests.Classes
{
    public class CasterTests
    {
        [Theory, AutoMoqData]
        public void Create()
        {
            var abilityService = new AbilityService(new TestXmlResourceLoader("cure.xml"));
            var cure = abilityService.GetAbilitiesWithName("cure");
            var caster = new Caster();
        }
    }

    public class Caster
    {
    }

    public class TestXmlResourceLoader : IResourceLoader
    {
        private readonly string fileName;

        public TestXmlResourceLoader(string fileName)
        {
            this.fileName = fileName;
        }

        public IEnumerable<XElement> LoadResources()
        {
            var xml = ParseResources.GetResource("cure.xml");
            return new List<XElement> { XDocument.Load(new StringReader(xml)).Root };
        }
    }

    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute()
            : base(new Fixture()
                .Customize(new AutoMoqCustomization()))
        {
        }
    }

}
