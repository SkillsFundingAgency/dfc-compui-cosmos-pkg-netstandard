using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace DFC.Compui.Cosmos.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class UrlPathAttribute : ValidationAttribute
    {
        private const string FieldNotUrlPath = "The field {0} does not contains valid characters for a url path.";

        protected override ValidationResult IsValid(object? value, ValidationContext? validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (validationContext == null)
            {
                throw new ArgumentNullException(nameof(validationContext));
            }

            var validChars = "abcdefghijklmnopqrstuvwxyz01234567890_-";
            var result = value switch
            {
                IEnumerable<string> list => list.All(x => x.Length > 0 && x.All(y => validChars.Contains(y, StringComparison.OrdinalIgnoreCase))),
                _ => value.ToString().All(x => validChars.Contains(x, StringComparison.OrdinalIgnoreCase)),
            };

            return result ? ValidationResult.Success
                : new ValidationResult(string.Format(CultureInfo.InvariantCulture, FieldNotUrlPath, validationContext.DisplayName), new[] { validationContext.MemberName });
        }
    }
}
