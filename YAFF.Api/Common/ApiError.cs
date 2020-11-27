namespace YAFF.Api.Common
{
    public class ApiError
    {
        public int Status { get; }
        public string Field { get; }
        public string Message { get; }

        public ApiError(int status, string field, string message)
        {
            Status = status;
            Field = field;
            Message = message;
        }
    }
}