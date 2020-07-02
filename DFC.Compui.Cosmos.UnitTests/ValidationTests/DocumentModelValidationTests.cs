using DFC.Compui.Cosmos.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using Xunit;

namespace DFC.Compui.Cosmos.UnitTests.ValidationTests
{
    [Trait("Category", "DocumentModel Validation Unit Tests")]
    public class DocumentModelValidationTests
    {
        private const string FieldInvalidGuid = "The field {0} has to be a valid GUID and cannot be an empty GUID.";

        private const string GuidEmpty = "00000000-0000-0000-0000-000000000000";

        [Fact]
        public void CheckForMissingMandatoryFields()
        {
            // Arrange
            var model = new TestDocumentModel();

            // Act
            var vr = Validate(model);

            // Assert
            Assert.True(vr.Count == 1);
            Assert.Contains(vr, c => c.MemberNames.Any(a => a == nameof(model.Id)));
        }

        [Theory]
        [InlineData(null)]
        [InlineData(GuidEmpty)]
        public void CanCheckIfDocumentIdIsInvalid(Guid documentId)
        {
            // Arrange
            var model = CreateModel(documentId);

            // Act
            var vr = Validate(model);

            // Assert
            Assert.True(vr.Count == 1);
            Assert.NotNull(vr.First(f => f.MemberNames.Any(a => a == nameof(model.Id))));
            Assert.Equal(string.Format(CultureInfo.InvariantCulture, FieldInvalidGuid, nameof(model.Id)), vr.First(f => f.MemberNames.Any(a => a == nameof(model.Id))).ErrorMessage);
        }

        private TestDocumentModel CreateModel(Guid id)
        {
            var model = new TestDocumentModel
            {
                Id = id,
            };

            return model;
        }

        private List<ValidationResult> Validate(TestDocumentModel model)
        {
            var vr = new List<ValidationResult>();
            var vc = new ValidationContext(model);
            Validator.TryValidateObject(model, vc, vr, true);

            return vr;
        }
    }
}
