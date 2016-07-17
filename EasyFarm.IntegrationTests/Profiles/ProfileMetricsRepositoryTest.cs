using System;
using System.IO;
using EasyFarm.DataSource.Database;
using EasyFarm.DataSource.Metrics;
using EasyFarm.DataSource.Profiles;
using SimpleInjector;
using Xunit;

namespace EasyFarm.IntegrationTests.Profiles
{
    public class ProfileMetricsRepositoryTest : IDisposable
    {
        private readonly Container _container = new Container();

        public ProfileMetricsRepositoryTest()
        {
            _container.Register<IDatabase>(() => new SqliteDatabase("EasyFarm.db"));
            _container.Register<IConnectionFactory>(() => new SqliteConnectionFactory("Data source=EasyFarm.db; Version=3"));
            _container.RegisterDecorator<IProfileRepository, ProfileMetricsRepository>();
            _container.Register<IProfileRepository, ProfileRepository>();
            _container.Register<IMetricsRepository, MetricsRepository>();
            _container.Register<IMetrics, Metrics>();
            _container.Register<ITableSetup, TableSetup>();

            var database = _container.GetInstance<IDatabase>();
            database.Create();

            var tableSetup = _container.GetInstance<ITableSetup>();
            tableSetup.Initialize();
        }

        [Fact]
        public void TestMethod()
        {
            // Fixture setup
            var sut = _container.GetInstance<IProfileRepository>();
            var metricsRepository = _container.GetInstance<IMetricsRepository>();

            // Exercise system
            sut.CreateProfile(new Profile());

            // Verify outcome            
            var result = metricsRepository.GetTimeMetric("profile.create.time").TimeSpan.TotalMilliseconds;
            Assert.True(result > 0);

            // Teardown
        }

        public void Dispose()
        {
            _container.Dispose();
            File.Delete("EasyFarm.db");
        }
    }
}