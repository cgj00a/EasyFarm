using EasyFarm.DataSource.Metrics;

namespace EasyFarm.DataSource.Profiles
{
    public class ProfileMetricsRepository : IProfileRepository
    {
        private readonly IMetrics _metrics;
        private readonly IProfileRepository _profileRepository;

        public ProfileMetricsRepository(
            IMetrics metrics,
            IProfileRepository profileRepository)
        {
            _metrics = metrics;
            _profileRepository = profileRepository;
        }

        public void CreateProfile(Profile profile)
        {
            using (_metrics.StartTimer("profile.create.time"))
            {
                _profileRepository.CreateProfile(profile);
            }            
        }

        public Profile FindProfileByName(string profileName)
        {
            return _profileRepository.FindProfileByName(profileName);
        }
    }
}