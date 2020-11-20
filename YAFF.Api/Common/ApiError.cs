namespace YAFF.Api.Common
{
    public class ApiError
    {
        public int StatusCode { get; }
        public string Field { get; }
        public string Message { get; }

        public ApiError(int statusCode, string field, string message)
        {
            StatusCode = statusCode;
            Field = field;
            Message = message;
        }
    }
}