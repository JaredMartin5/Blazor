using Shared.Common;

namespace Client.Helpers;

public static class ErrorResultHelper
{
    public static List<ApiError> CreateGeneralError()
    {
        return [new("general", "Failed to parse server response.")];
    }
}
