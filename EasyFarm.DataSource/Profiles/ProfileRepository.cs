using Dapper;
using EasyFarm.DataSource.Database;

namespace EasyFarm.DataSource.Profiles
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public ProfileRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void CreateProfile(Profile profile)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.ExecuteScalar(@"
                    insert into profiles (name, engage) 
                    values (@name, @engage)",
                    new
                    {
                        name = profile.Name,
                        engage = profile.Engage
                    });
            }
        }

        public Profile FindProfileByName(string profileName)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                return connection.QueryFirst<Profile>(@"
                    select * from profiles
                    where name = @name", new {name = profileName});
            }
        }
    }
}