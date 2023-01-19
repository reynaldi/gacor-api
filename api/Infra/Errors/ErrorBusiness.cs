namespace GacorAPI.Infra.Errors
{
    public class ErrorBusiness
    {
        public string Message { get; set; }
        public ErrorCode ErrorCode { get; set; }
    }

    public enum ErrorCode
    {
        UserAlreadyRegistered = 1,
        UserNotFound
    }

    public static class ErrorGenerator
    {
        public static ErrorBusiness Generate(ErrorCode code)
        {
            return new ErrorBusiness
            {
                ErrorCode = code,
                Message = generateMessage(code)
            };
        }

        private static string generateMessage(ErrorCode code)
        {
            switch (code)
            {
                case ErrorCode.UserAlreadyRegistered:
                    return "User already registered";
                case ErrorCode.UserNotFound:
                    return "User not found";
                default:
                    return "";
            }
        }
    }
}