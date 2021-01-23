namespace YAFF.Api.Common
{
    public class ApiError
    {
        public string Field { get; }
        public string Message { get; }

        public ApiError(string field, string message)
        {
            Field = field;
            Message = message;
        }
    }
}