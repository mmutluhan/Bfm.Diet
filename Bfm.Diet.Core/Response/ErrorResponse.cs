namespace Bfm.Diet.Core.Response
{
    public class ErrorResponse : Response
    {
        public string ExceptionMessage { get; set; }
        public ErrorResponse(string message) : base(false, message)
        {
        }
        public ErrorResponse(string exceptionMessage, string message) : base(false, message)
        {
            ExceptionMessage = exceptionMessage;
        }

        public ErrorResponse() : base(false)
        {
        }
    }
}