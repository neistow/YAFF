namespace YAFF.Api.Common
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; }
        public T Data { get; }

        public ApiResponse(int statusCode, T data)
        {
            StatusCode = statusCode;
            Data = data;
        }
    }
}