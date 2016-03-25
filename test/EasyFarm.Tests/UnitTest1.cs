using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EasyFarm.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void LoadsProfileWithCorrectName()
        {
            var controller = new Controller();
            var profile = controller.LoadProfile("Mike");
            Assert.AreEqual("Mike", profile.Name);
        }

        [TestMethod]
        public void SetsProcessForProfile()
        {
            var controller = new Controller();
            var profile = controller.LoadProfile("Mike");
            var process = new Process();
            profile.Process = process;
            Assert.AreEqual(profile.Process, process);
        }
    }

    public class Controller
    {
        public Profile LoadProfile(string profile)
        {
            return new Profile() {Name = profile};
        }
    }

    public class Profile
    {
        public string Name { get; set; }
        public Process Process { get; set; }
    }
}
