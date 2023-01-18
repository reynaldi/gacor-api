using GacorAPI.Infra.Body;

namespace GacorAPI.Infra.Errors
{
    public class ErrorBody : ResponseBody
    {
        public ErrorType Error { get; set; }
    }

    public static class ErrorBodyGenerator
    {
        public static ErrorBody Generate(ErrorBusiness error)
        {
            return new ErrorBody
            {
                Error = new ErrorType
                {
                    ErrorCode = (int)error.ErrorCode,
                    ErrorMessage = error.Message
                }
            };
        }
    }
}