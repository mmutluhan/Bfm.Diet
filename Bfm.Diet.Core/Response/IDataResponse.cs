namespace Bfm.Diet.Core.Response
{
    public interface IDataResponse<out T> : IResponse
    {
        T Data { get; }
    }
}