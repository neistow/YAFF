using System.Collections.Generic;

namespace YAFF.Api.Common
{
    public class ApiError
    {
        public IDictionary<string, IEnumerable<string>> Errors { get; }

        public ApiError(string field, string error)
        {
            Errors = new Dictionary<string, IEnumerable<string>>
            {
                {field, new[] {error}}
            };
        }
    }
}