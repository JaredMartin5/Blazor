using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Concurrent;

namespace Client
{
    public static class ValidationHelper
    {
        private static readonly ConcurrentDictionary<EditContext, ValidationMessageStore> _validationMessageStores =
            new();

        public static void AddValidationError(EditContext editContext, object model, string propertyName,
            string errorMessage)
        {
            var messageStore =
                _validationMessageStores.GetOrAdd(editContext, context => new ValidationMessageStore(context));

            var fieldIdentifier = new FieldIdentifier(model, propertyName);

            messageStore.Clear(fieldIdentifier);
            messageStore.Add(fieldIdentifier, errorMessage);

            editContext.NotifyValidationStateChanged();
        }

        public static void ClearAllValidationMessages(EditContext editContext)
        {
            if (_validationMessageStores.TryGetValue(editContext, out var messageStore))
            {
                messageStore.Clear();
                editContext.NotifyValidationStateChanged();
            }
        }
    }
}