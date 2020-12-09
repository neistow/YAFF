namespace YAFF.Core.Common
{
    /// <summary>
    /// Class that represent a result of an operation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result<T>
    {
        public bool Succeeded { get; }
        public string Field { get; }
        public string Message { get; }
        public T Data { get; }

        private Result(T data, bool succeeded, string field, string message)
        {
            Data = data;
            Succeeded = succeeded;
            Field = field;
            Message = message;
        }


        public static Result<T> Success(T data = default)
        {
            return new Result<T>(data, true, string.Empty, string.Empty);
        }

        public static Result<T> Failure(string field = "", string message = "")
        {
            return new Result<T>(default, false, field, message);
        }
    }
}