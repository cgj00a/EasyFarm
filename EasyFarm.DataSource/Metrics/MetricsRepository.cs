using System;
using Dapper;
using EasyFarm.DataSource.Database;

namespace EasyFarm.DataSource.Metrics
{
    public class MetricsRepository : IMetricsRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public MetricsRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public TimeMetric GetTimeMetric(string metricName)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                return connection.QueryFirst<TimeMetric>(@"
                    select * from TimeMetrics
                    where name = @name", new { name = metricName});
            }
        }

        public void SetTimeMetric(string metricName, long elapsedTicks)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.ExecuteScalar(@"
                    insert into TimeMetrics(id, name, ticks)
                    values (@guid, @name, @ticks)", new
                {
                    guid = Guid.NewGuid(),
                    name = metricName,
                    ticks = elapsedTicks
                });
            }
        }
    }
}