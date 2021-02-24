using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Cosmos.UnitTests.Models;
using FakeItEasy;
using System;
using System.Net;
using Xunit;

namespace DFC.Compui.Cosmos.UnitTests.DocumentTests
{
    [Trait("Category", "Document Service Unit Tests")]
    public class DocumentServicePurgeTests
    {
        [Fact]
        public void DocumentPurgeReturnsSuccessWWhenDocumentsPurged()
        {
            // arrange
            const bool expectedResult = true;
            var repository = A.Fake<ICosmosRepository<TestDocumentModel>>();

            A.CallTo(() => repository.PurgeAsync()).Returns(HttpStatusCode.NoContent);

            var documentService = new DocumentService<TestDocumentModel>(repository);

            // act
            var result = documentService.PurgeAsync().Result;

            // assert
            A.CallTo(() => repository.PurgeAsync()).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public void DocumentPurgeReturnsFalseWhenMissingRepository()
        {
            // arrange
            const bool expectedResult = false;
            var repository = A.Dummy<ICosmosRepository<TestDocumentModel>>();

            A.CallTo(() => repository.PurgeAsync()).Returns(HttpStatusCode.FailedDependency);

            var documentService = new DocumentService<TestDocumentModel>(repository);

            // act
            var result = documentService.PurgeAsync().Result;

            // assert
            A.CallTo(() => repository.PurgeAsync()).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }
    }
}
