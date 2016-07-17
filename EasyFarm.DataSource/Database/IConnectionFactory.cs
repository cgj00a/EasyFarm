using System.Data;

namespace EasyFarm.DataSource.Database
{
    public interface IConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}