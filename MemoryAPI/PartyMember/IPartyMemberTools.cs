namespace MemoryAPI
{
    public interface IPartyMemberTools
    {        
        int ServerID { get; }
        int HPPCurrent { get; }
        IPosition Position { get; }
    }
}