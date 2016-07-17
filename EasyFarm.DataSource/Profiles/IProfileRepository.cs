namespace EasyFarm.DataSource.Profiles
{
    public interface IProfileRepository
    {
        void CreateProfile(Profile profile);
        Profile FindProfileByName(string profileName);
    }
}