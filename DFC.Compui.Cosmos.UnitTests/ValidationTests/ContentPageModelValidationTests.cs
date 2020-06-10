using DFC.Compui.Cosmos.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using Xunit;

namespace DFC.Compui.Cosmos.UnitTests.ValidationTests
{
    [Trait("Category", "ContentPageModel Validation Unit Tests")]
    public class ContentPageModelValidationTests
    {
        private const string FieldInvalidGuid = "The field {0} has to be a valid GUID and cannot be an empty GUID.";
        private const string FieldNotLowercase = "The field {0} is not in lowercase.";
        private const string FieldNotUrlPath = "The field {0} does not contains valid characters for a url path.";

        private const string GuidEmpty = "00000000-0000-0000-0000-000000000000";

        [Fact]
        public void CheckForMissingMandatoryFields()
        {
            // Arrange
            var model = new ContentPageModel();

            // Act
            var vr = Validate(model);

            // Assert
            Assert.True(vr.Count == 6);
            Assert.Contains(vr, c => c.MemberNames.Any(a => a == nameof(model.Id)));
            Assert.Contains(vr, c => c.MemberNames.Any(a => a == nameof(model.CanonicalName)));
            Assert.Contains(vr, c => c.MemberNames.Any(a => a == nameof(model.Version)));
            Assert.Contains(vr, c => c.MemberNames.Any(a => a == nameof(model.BreadcrumbTitle)));
            Assert.Contains(vr, c => c.MemberNames.Any(a => a == nameof(model.Url)));
            Assert.Contains(vr, c => c.MemberNames.Any(a => a == nameof(model.Content)));
        }

        [Theory]
        [InlineData(null)]
        [InlineData(GuidEmpty)]
        public void CanCheckIfDocumentIdIsInvalid(Guid documentId)
        {
            // Arrange
            var model = CreateModel(documentId, "canonicalname1", "content1", "abc-def", new List<string>());

            // Act
            var vr = Validate(model);

            // Assert
            Assert.True(vr.Count == 1);
            Assert.NotNull(vr.First(f => f.MemberNames.Any(a => a == nameof(model.Id))));
            Assert.Equal(string.Format(CultureInfo.InvariantCulture, FieldInvalidGuid, nameof(model.Id)), vr.First(f => f.MemberNames.Any(a => a == nameof(model.Id))).ErrorMessage);
        }

        [Theory]
        [InlineData("abcdefghijklmnopqrstuvwxyz")]
        [InlineData("0123456789")]
        [InlineData("abc")]
        [InlineData("xyz123")]
        [InlineData("abc_def")]
        [InlineData("abc-def")]
        public void CanCheckIfCanonicalNameIsValid(string canonicalName)
        {
            // Arrange
            var model = CreateModel(Guid.NewGuid(), canonicalName, "content", "abc-def", new List<string>());

            // Act
            var vr = Validate(model);

            // Assert
            Assert.True(vr.Count == 0);
        }

        [Theory]
        [InlineData("ABCDEF")]
        public void CanCheckIfCanonicalNameIsInvalid(string canonicalName)
        {
            // Arrange
            var model = CreateModel(Guid.NewGuid(), canonicalName, "content", "abc-def", new List<string>());

            // Act
            var vr = Validate(model);

            // Assert
            Assert.True(vr.Count > 0);
            Assert.NotNull(vr.First(f => f.MemberNames.Any(a => a == nameof(model.CanonicalName))));
            Assert.Equal(string.Format(CultureInfo.InvariantCulture, FieldNotLowercase, nameof(model.CanonicalName)), vr.First(f => f.MemberNames.Any(a => a == nameof(model.CanonicalName))).ErrorMessage);
        }

        [Theory]
        [InlineData("abcdefghijklmnopqrstuvwxyz")]
        [InlineData("0123456789")]
        [InlineData("abc")]
        [InlineData("xyz123")]
        [InlineData("abc_def")]
        [InlineData("abc-def")]
        public void CanCheckIfAlternativeNameIsValid(string alternativeName)
        {
            // Arrange
            var model = CreateModel(Guid.NewGuid(), "canonicalname1", "content1", "abc-def", new List<string>() { alternativeName });

            // Act
            var vr = Validate(model);

            // Assert
            Assert.True(vr.Count == 0);
        }

        [Theory]
        [InlineData("ABCDEF")]
        public void CanCheckIfAlternativeNameIsInvalid(string alternativeName)
        {
            // Arrange
            var model = CreateModel(Guid.NewGuid(), "canonicalname1", "content1", "abc-def", new List<string>() { alternativeName });

            // Act
            var vr = Validate(model);

            // Assert
            Assert.True(vr.Count > 0);
            Assert.NotNull(vr.First(f => f.MemberNames.Any(a => a == nameof(model.AlternativeNames))));
            Assert.Equal(string.Format(CultureInfo.InvariantCulture, FieldNotLowercase, nameof(model.AlternativeNames)), vr.First(f => f.MemberNames.Any(a => a == nameof(model.AlternativeNames))).ErrorMessage);
        }

        [Theory]
        [InlineData("abcdefghijklmnopqrstuvwxyz")]
        [InlineData("0123456789")]
        [InlineData("abc")]
        [InlineData("xyz123")]
        [InlineData("abc_def")]
        [InlineData("abc-def")]
        [InlineData("/abc-def")]
        [InlineData("/abc/def")]
        public void CanCheckIfUrlIsValid(string url)
        {
            // Arrange
            var model = CreateModel(Guid.NewGuid(), "canonicalname1", "content1", url, new List<string>());

            // Act
            var vr = Validate(model);

            // Assert
            Assert.True(vr.Count == 0);
        }

        [Theory]
        [InlineData("abc def")]
        public void CanCheckIfUrlIsInvalid(string url)
        {
            // Arrange
            var model = CreateModel(Guid.NewGuid(), "canonicalname1", "content1", url, new List<string>());

            // Act
            var vr = Validate(model);

            // Assert
            Assert.True(vr.Count > 0);
            Assert.NotNull(vr.First(f => f.MemberNames.Any(a => a == nameof(model.Url))));
            Assert.Equal(string.Format(CultureInfo.InvariantCulture, FieldNotUrlPath, nameof(model.Url)), vr.First(f => f.MemberNames.Any(a => a == nameof(model.Url))).ErrorMessage);
        }

        private ContentPageModel CreateModel(Guid id, string canonicalName, string content, string url, List<string> alternativeNames)
        {
            var model = new ContentPageModel
            {
                Id = id,
                CanonicalName = canonicalName,
                BreadcrumbTitle = canonicalName,
                Version = Guid.NewGuid(),
                Url = new Uri(url, UriKind.Relative),
                Content = content,
                AlternativeNames = alternativeNames.ToArray(),
                LastReviewed = DateTime.UtcNow,
            };

            return model;
        }

        private List<ValidationResult> Validate(ContentPageModel model)
        {
            var vr = new List<ValidationResult>();
            var vc = new ValidationContext(model);
            Validator.TryValidateObject(model, vc, vr, true);

            return vr;
        }
    }
}
