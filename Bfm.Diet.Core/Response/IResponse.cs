namespace Bfm.Diet.Core.Response
{
    public interface IResponse
    {
        bool Success { get; }
        string Message { get; }
    }
}