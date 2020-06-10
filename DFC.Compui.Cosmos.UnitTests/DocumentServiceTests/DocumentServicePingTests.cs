using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Cosmos.UnitTests.Models;
using FakeItEasy;
using Xunit;

namespace DFC.Compui.Cosmos.UnitTests.DocumentTests
{
    [Trait("Category", "Document Service Unit Tests")]
    public class DocumentServicePingTests
    {
        [Fact]
        public void DocumentPingReturnsSuccess()
        {
            // arrange
            var repository = A.Fake<ICosmosRepository<TestDocumentModel>>();
            var expectedResult = true;

            A.CallTo(() => repository.PingAsync()).Returns(expectedResult);

            var documentService = new DocumentService<TestDocumentModel>(repository);

            // act
            var result = documentService.PingAsync().Result;

            // assert
            A.CallTo(() => repository.PingAsync()).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public void DocumentPingReturnsFalseWhenMissingRepository()
        {
            // arrange
            var repository = A.Dummy<ICosmosRepository<TestDocumentModel>>();
            var expectedResult = false;

            A.CallTo(() => repository.PingAsync()).Returns(expectedResult);

            var documentService = new DocumentService<TestDocumentModel>(repository);

            // act
            var result = documentService.PingAsync().Result;

            // assert
            A.CallTo(() => repository.PingAsync()).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }
    }
}
