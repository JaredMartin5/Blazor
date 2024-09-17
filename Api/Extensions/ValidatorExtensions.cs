using FluentValidation.Results;
using Shared.Common;

namespace Api.Extensions;

public static class ValidatorExtensions
{
    public static ApiErrorResult GetApiErrorResult(this ValidationResult validationResult)
    {
        return new ApiErrorResult(validationResult.Errors.Select(x => new ApiError(x.PropertyName, x.ErrorMessage)));
    }
}
