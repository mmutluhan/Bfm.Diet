namespace Bfm.Diet.Core.Response
{
    public class Response : IResponse
    {
        public Response(bool success, string message) : this(success)
        {
            Message = message;
        }

        public Response(bool success)
        {
            Success = success;
        }

        public bool Success { get; }
        public string Message { get; }
    }
}