namespace Bfm.Diet.Core.Session
{
    public interface IBfmSessionInfo
    {
        int Id { get; }
        string Adi { get; }
        string EMail { get; }
        string SessionId { get; }

    }
}