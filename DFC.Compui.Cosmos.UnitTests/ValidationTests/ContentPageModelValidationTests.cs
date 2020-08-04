using DFC.Compui.Cosmos.UnitTests.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using Xunit;

namespace DFC.Compui.Cosmos.UnitTests.ValidationTests
{
    [Trait("Category", "TestContentPageModel Validation Unit Tests")]
    public class ContentPageModelValidationTests
    {
        private const string FieldInvalidGuid = "The field {0} has to be a valid GUID and cannot be an empty GUID.";
        private const string FieldNotLowercase = "The field {0} is not in lowercase.";

        private const string GuidEmpty = "00000000-0000-0000-0000-000000000000";

        [Fact]
        public void CheckForMissingMandatoryFields()
        {
            // Arrange
            var model = new TestContentPageModel();

            // Act
            var vr = Validate(model);

            // Assert
            Assert.Equal(5, vr.Count);
            Assert.Contains(vr, c => c.MemberNames.Any(a => a == nameof(model.Id)));
            Assert.Contains(vr, c => c.MemberNames.Any(a => a == nameof(model.CanonicalName)));
            Assert.Contains(vr, c => c.MemberNames.Any(a => a == nameof(model.Version)));
            Assert.Contains(vr, c => c.MemberNames.Any(a => a == nameof(model.Url)));
            Assert.Contains(vr, c => c.MemberNames.Any(a => a == nameof(model.Content)));
        }

        [Theory]
        [InlineData(null)]
        [InlineData(GuidEmpty)]
        public void CanCheckIfDocumentIdIsInvalid(Guid documentId)
        {
            // Arrange
            var model = CreateModel(documentId, "canonicalname1", "pagelocation", "content1", "abc-def", new List<string>());

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
            var model = CreateModel(Guid.NewGuid(), canonicalName, "pagelocation", "content", "abc-def", new List<string>());

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
            var model = CreateModel(Guid.NewGuid(), canonicalName, "pagelocation", "content", "abc-def", new List<string>());

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
        public void CanCheckIfPageLocationeIsValid(string pagelocation)
        {
            // Arrange
            var model = CreateModel(Guid.NewGuid(), "canonicalname1", pagelocation, "content", "abc-def", new List<string>());

            // Act
            var vr = Validate(model);

            // Assert
            Assert.True(vr.Count == 0);
        }

        [Theory]
        [InlineData("ABCDEF")]
        public void CanCheckIfPageLocationIsInvalid(string pagelocation)
        {
            // Arrange
            var model = CreateModel(Guid.NewGuid(), "canonicalname1", pagelocation, "content", "abc-def", new List<string>());

            // Act
            var vr = Validate(model);

            // Assert
            Assert.True(vr.Count > 0);
            Assert.NotNull(vr.First(f => f.MemberNames.Any(a => a == nameof(model.PageLocation))));
            Assert.Equal(string.Format(CultureInfo.InvariantCulture, FieldNotLowercase, nameof(model.PageLocation)), vr.First(f => f.MemberNames.Any(a => a == nameof(model.PageLocation))).ErrorMessage);
        }

        [Theory]
        [InlineData("abcdefghijklmnopqrstuvwxyz")]
        [InlineData("0123456789")]
        [InlineData("abc")]
        [InlineData("xyz123")]
        [InlineData("abc_def")]
        [InlineData("abc-def")]
        public void CanCheckIfRedirectLocationIsValid(string redirectLocation)
        {
            // Arrange
            var model = CreateModel(Guid.NewGuid(), "canonicalname1", "pagelocation", "content1", "abc-def", new List<string>() { redirectLocation });

            // Act
            var vr = Validate(model);

            // Assert
            Assert.True(vr.Count == 0);
        }

        [Theory]
        [InlineData("ABCDEF")]
        public void CanCheckIfRedirectLocationIsInvalid(string redirectLocation)
        {
            // Arrange
            var model = CreateModel(Guid.NewGuid(), "canonicalname1", "pagelocation", "content1", "abc-def", new List<string>() { redirectLocation });

            // Act
            var vr = Validate(model);

            // Assert
            Assert.True(vr.Count > 0);
            Assert.NotNull(vr.First(f => f.MemberNames.Any(a => a == nameof(model.RedirectLocations))));
            Assert.Equal(string.Format(CultureInfo.InvariantCulture, FieldNotLowercase, nameof(model.RedirectLocations)), vr.First(f => f.MemberNames.Any(a => a == nameof(model.RedirectLocations))).ErrorMessage);
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
        [InlineData("https://abc/def")]
        public void CanCheckIfUrlIsValid(string url)
        {
            // Arrange
            var model = CreateModel(Guid.NewGuid(), "canonicalname1", "pagelocation", "content1", url, new List<string>());

            // Act
            var vr = Validate(model);

            // Assert
            Assert.True(vr.Count == 0);
        }

        private TestContentPageModel CreateModel(Guid id, string canonicalName, string pageLocation, string content, string url, List<string> redirectLocations)
        {
            var model = new TestContentPageModel
            {
                Id = id,
                CanonicalName = canonicalName,
                PageLocation = pageLocation,
                Version = Guid.NewGuid(),
                Url = new Uri(url, UriKind.RelativeOrAbsolute),
                Content = content,
                RedirectLocations = redirectLocations.ToArray(),
                LastReviewed = DateTime.UtcNow,
            };

            return model;
        }

        private List<ValidationResult> Validate(TestContentPageModel model)
        {
            var vr = new List<ValidationResult>();
            var vc = new ValidationContext(model);
            Validator.TryValidateObject(model, vc, vr, true);

            return vr;
        }
    }
}
