using GacorAPI.Infra.Errors;

namespace GacorAPI.Infra.Body
{
    public class ResponseBody
    {
        public object Result { get; set; }
    }

    public static class ResponseBodyGenerator
    {
        public static ResponseBody Generate(object data, ErrorBusiness error)
        {
            var response = new ResponseBody();
            if(error != null)
            {
                response = ErrorBodyGenerator.Generate(error);
            }
            response.Result = data;
            return response;
        }
    }
}