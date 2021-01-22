using System;
using YAFF.Api.Common;
using YAFF.Core.Common;

namespace YAFF.Api.Extensions
{
    public static class ResultExtensions
    {
        public static ApiError ToApiError<T>(this Result<T> result, int statusCode)
        {
            if (result.Succeeded)
            {
                throw new InvalidOperationException("Result has succeeded");
            }

            return new ApiError(statusCode, result.Field, result.Message);
        }

        public static T ToApiResponse<T>(this Result<T> result)
        {
            if (!result.Succeeded)
            {
                throw new InvalidOperationException("Result has failed");
            }

            return result.Data;
        }
    }
}