using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Cosmos.UnitTests.Models;
using FakeItEasy;
using System;
using Xunit;

namespace DFC.Compui.Cosmos.UnitTests.DocumentTests
{
    [Trait("Category", "Document Service Unit Tests")]
    public class DocumentServiceGetByIdTests
    {
        [Fact]
        public void DocumentGetByIdReturnsSuccess()
        {
            // arrange
            Guid documentId = Guid.NewGuid();
            var repository = A.Fake<ICosmosRepository<TestDocumentModel>>();
            var expectedResult = A.Fake<TestDocumentModel>();

            A.CallTo(() => repository.GetByIdAsync(A<Guid>.Ignored)).Returns(expectedResult);

            var documentService = new DocumentService<TestDocumentModel>(repository);

            // act
            var result = documentService.GetByIdAsync(documentId).Result;

            // assert
            A.CallTo(() => repository.GetByIdAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public void DocumentGetByIdReturnsNullWhenMissingRepository()
        {
            // arrange
            Guid documentId = Guid.NewGuid();
            var repository = A.Fake<ICosmosRepository<TestDocumentModel>>();
            TestDocumentModel? expectedResult = null;

            A.CallTo(() => repository.GetByIdAsync(A<Guid>.Ignored)).Returns(expectedResult);

            var documentService = new DocumentService<TestDocumentModel>(repository);

            // act
            var result = documentService.GetByIdAsync(documentId).Result;

            // assert
            A.CallTo(() => repository.GetByIdAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }
    }
}
