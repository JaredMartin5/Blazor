using Shared.Common;

namespace Client.Helpers
{
    public class ErrorResultHelper
    {
        public static async Task<ApiResult<T>> CreateErrorResult<T>(string propertyName)
            where T : class
        {
            return await Task.FromResult(
                new ApiResult<T>
                {
                    Successful = false,
                    Errors = new List<Error> { new(propertyName, "Failed to parse server response.") }
                }
            );
        }
    }
}
