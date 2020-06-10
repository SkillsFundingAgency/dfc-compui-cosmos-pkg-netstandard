using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace DFC.Compui.Cosmos.Contracts
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class GuidAttribute : ValidationAttribute
    {
        private const string FieldInvalidGuid = "The field {0} has to be a valid GUID and cannot be an empty GUID.";

        protected override ValidationResult IsValid(object? value, ValidationContext? validationContext)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (validationContext == null)
            {
                throw new ArgumentNullException(nameof(validationContext));
            }

            if (!Guid.TryParse(value.ToString(), out var guid) || guid == Guid.Empty)
            {
                return new ValidationResult(string.Format(CultureInfo.InvariantCulture, FieldInvalidGuid, validationContext.DisplayName), new[] { validationContext.MemberName });
            }

            return ValidationResult.Success;
        }
    }
}
