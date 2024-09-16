namespace Shared.Common;

public record ApiError(string Property, string Message);

public record ApiErrorResult(IEnumerable<ApiError> Errors);
