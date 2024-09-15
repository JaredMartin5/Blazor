using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using OneOf;
using Shared.Common;

namespace Client.Helpers;

public class FormSubmissionHelper
{
    private readonly NavigationManager _navigationManager;

    public FormSubmissionHelper(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
    }

    public async Task HandleFormSubmission<TModel, TResult>(EditContext editContext, TModel model, Func<TModel, Task<OneOf<TResult, List<ApiError>>>> submitFunc, string successNavigationUrl)
    {
        ValidationHelper.ClearAllValidationMessages(editContext);
        editContext.Validate();

        var result = await submitFunc(model);

        result.Switch(
            success =>
            {
                _navigationManager.NavigateTo(successNavigationUrl);
            },
            errors =>
            {
                if (Equals(model, null))
                {
                    throw new Exception("Data is null");
                }
                foreach (var error in errors)
                {
                    var correctedPropertyName = error.Property[..1].ToUpper() + error.Property[1..];
                    ValidationHelper.AddValidationError(editContext, model, correctedPropertyName, error.Message);
                }

                editContext.NotifyValidationStateChanged();
            }
        );
    }
}
