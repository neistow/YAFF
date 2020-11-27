namespace YAFF.Api.Common
{
    public class ApiResponse<T>
    {
        public int Status { get; }
        public T Data { get; }

        public ApiResponse(int status, T data)
        {
            Status = status;
            Data = data;
        }
    }
}