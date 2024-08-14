using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Shared.Common;

namespace Client.Helpers;

public class FormSubmissionHelper
{
    private readonly NavigationManager _navigationManager;

    public FormSubmissionHelper(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
    }

    public async Task HandleFormSubmission<TModel, TResult>(
        EditContext editContext,
        TModel model,
        Func<TModel, Task<ApiResult<TResult>>> submitFunc,
        string successNavigationUrl) where TResult : class
    {
        ValidationHelper.ClearAllValidationMessages(editContext);
        editContext.Validate();

        var result = await submitFunc(model);

        if (!result.Successful)
        {
            foreach (var error in result.Errors)
            {
                var correctedPropertyName = error.Property[..1].ToUpper() + error.Property[1..];
                ValidationHelper.AddValidationError(editContext, model, correctedPropertyName, error.Message);
            }

            editContext.NotifyValidationStateChanged();
        }
        else
        {
            _navigationManager.NavigateTo(successNavigationUrl);
        }
    }
}