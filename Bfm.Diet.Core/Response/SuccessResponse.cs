namespace Bfm.Diet.Core.Response
{
    public class SuccessResponse : Response
    {
        public SuccessResponse(string message) : base(true, message)
        {
        }

        public SuccessResponse() : base(true)
        {
        }
    }
}