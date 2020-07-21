using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Cosmos.UnitTests.Models;
using FakeItEasy;
using System;
using System.Net;
using Xunit;

namespace DFC.Compui.Cosmos.UnitTests.DocumentTests
{
    [Trait("Category", "Document Service Unit Tests")]
    public class DocumentServiceDeleteTests
    {
        [Fact]
        public void DocumentDeleteReturnsSuccessWWhenDocumentDeleted()
        {
            // arrange
            const bool expectedResult = true;
            Guid documentId = Guid.NewGuid();
            var repository = A.Fake<ICosmosRepository<TestDocumentModel>>();

            A.CallTo(() => repository.DeleteAsync(documentId)).Returns(HttpStatusCode.NoContent);

            var documentService = new DocumentService<TestDocumentModel>(repository);

            // act
            var result = documentService.DeleteAsync(documentId).Result;

            // assert
            A.CallTo(() => repository.DeleteAsync(documentId)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public void DocumentDeleteReturnsNullWWhenDocumentNotDeleted()
        {
            // arrange
            const bool expectedResult = false;
            Guid documentId = Guid.NewGuid();
            var repository = A.Fake<ICosmosRepository<TestDocumentModel>>();

            A.CallTo(() => repository.DeleteAsync(documentId)).Returns(HttpStatusCode.BadRequest);

            var documentService = new DocumentService<TestDocumentModel>(repository);

            // act
            var result = documentService.DeleteAsync(documentId).Result;

            // assert
            A.CallTo(() => repository.DeleteAsync(documentId)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public void DocumentDeleteReturnsFalseWhenMissingRepository()
        {
            // arrange
            const bool expectedResult = false;
            Guid documentId = Guid.NewGuid();
            var repository = A.Dummy<ICosmosRepository<TestDocumentModel>>();

            A.CallTo(() => repository.DeleteAsync(documentId)).Returns(HttpStatusCode.FailedDependency);

            var documentService = new DocumentService<TestDocumentModel>(repository);

            // act
            var result = documentService.DeleteAsync(documentId).Result;

            // assert
            A.CallTo(() => repository.DeleteAsync(documentId)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }
    }
}
