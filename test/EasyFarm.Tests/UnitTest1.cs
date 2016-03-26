using System.IO;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace EasyFarm.Tests
{
    public class ProfileTests
    {
        public class Create
        {
            [Theory, AutoMoqData]
            public void CreatesProfileWithGivenName(Controller controller)
            {
                var profile = controller.CreateProfile("Mike");
                Assert.Equal("Mike", profile.Name);
            }

            [Theory, AutoMoqData]
            public void SaveProfileWithChanges(
                Controller controller, 
                Profile profile)
            {
                var result = controller.SaveProfile(profile);
                Assert.True(result.Success);
            }
        }
    }

    public class ProfileManager
    {
        public Result Save(Profile profile)
        {
            if (File.Exists(profile.Name)) return Result.Fail("File already exists");
            return Result.Ok();
        }
    }

    public class Controller
    {
        private readonly ProfileManager _profileManager;

        public Controller(ProfileManager profileManager)
        {
            _profileManager = profileManager;
        }

        public Profile CreateProfile(string profileName)
        {
            return new Profile { Name = profileName };
        }

        public Result SaveProfile(Profile profile)
        {
            return _profileManager.Save(profile);
        }
    }

    public class Result
    {
        public bool Success { get; }

        public string Message { get; private set; }

        public Result(bool success, string message = "")
        {
            Success = success;
            Message = message;
        }        

        public static Result Fail(string message)
        {
            return new Result(false, message);
        }

        public static Result Ok()
        {
            return new Result(true);
        }
    }

    public class Profile
    {
        public string Name { get; set; }
    }

    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute() :
            base(new Fixture().Customize(new AutoMoqCustomization()))
        {
        }
    }
}
