using System;
using System.IO;
using EasyFarm.DataSource.Database;
using EasyFarm.DataSource.Profiles;
using SimpleInjector;
using Xunit;

namespace EasyFarm.IntegrationTests.Profiles
{
    public class ProfileRepositoryTest : IDisposable
    {
        private readonly Container _container = new Container();

        public ProfileRepositoryTest()
        {
            _container.Register<IDatabase>(() => new SqliteDatabase("ProfileRepositoryTest.db"));
            _container.Register<IConnectionFactory>(() => new SqliteConnectionFactory("Data source=ProfileRepositoryTest.db; Version=3"));
            _container.Register<IProfileRepository, ProfileRepository>();
            _container.Register<ITableSetup, TableSetup>();

            var database = _container.GetInstance<IDatabase>();
            database.Create();

            var tableSetup = _container.GetInstance<ITableSetup>();
            tableSetup.Initialize();
        }

        [Fact]
        public void CreateProfileWithNewProfileWillStoreIt()
        {
            // Fixture setup
            var sut = _container.GetInstance<IProfileRepository>();

            // Exercise system            
            sut.CreateProfile(new Profile { Name = "Mykezero", Engage = true });

            // Verify outcome
            var result = sut.FindProfileByName("Mykezero");
            Assert.Equal("Mykezero", result.Name);
            Assert.True(result.Engage);

            // Teardown
        }

        public void Dispose()
        {
            _container.Dispose();
            File.Delete("ProfileRepositoryTest.db");
        }
    }
}