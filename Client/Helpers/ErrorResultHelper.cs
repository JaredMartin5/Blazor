using Shared.Common;

namespace Client.Helpers;

public static class ErrorResultHelper
{
    public static ApiErrorResult CreateGeneralError()
    {
        return new ApiErrorResult([new("general", "Failed to parse server response.")]);
    }
}
