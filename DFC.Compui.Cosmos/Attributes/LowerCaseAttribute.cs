using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace DFC.Compui.Cosmos.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class LowerCaseAttribute : ValidationAttribute
    {
        private const string FieldNotLowercase = "The field {0} is not in lowercase.";

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

            var result = false;

            switch (value)
            {
                case IEnumerable<string> list:
                    result = list.All(s => s.Equals(s.ToLowerInvariant(), StringComparison.Ordinal));
                    break;
                default:
                    string item = value.ToString() ?? string.Empty;
                    result = item.Equals(item.ToLowerInvariant(), StringComparison.Ordinal);
                    break;
            }

            return result ? ValidationResult.Success
                : new ValidationResult(string.Format(CultureInfo.InvariantCulture, FieldNotLowercase, validationContext.DisplayName), new[] { validationContext.MemberName });
        }
    }
}
