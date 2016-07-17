using Dapper;

namespace EasyFarm.DataSource.Database
{
    public class TableSetup : ITableSetup
    {
        private readonly IConnectionFactory _connectionFactory;

        public TableSetup(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void Initialize()
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.ExecuteScalar(@"
                create table profiles(
                    id guid primary key, 
                    name text, 
                    engage bit)");

                connection.ExecuteScalar(@"create table TimeMetrics(
                    id guid primary key, 
                    name text, 
                    ticks integer
                )");
            }
        }
    }
}