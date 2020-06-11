using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Cosmos.UnitTests.Models;
using FakeItEasy;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Compui.Cosmos.UnitTests.DocumentTests
{
    [Trait("Category", "Document Service Unit Tests")]
    public class DocumentServiceGetAllTests
    {
        [Fact]
        public async Task DocumentGetAllListReturnsSuccess()
        {
            // arrange
            var repository = A.Fake<ICosmosRepository<TestDocumentModel>>();
            var expectedResults = A.CollectionOfFake<TestDocumentModel>(2);

            A.CallTo(() => repository.GetAllAsync()).Returns(expectedResults);

            var documentService = new DocumentService<TestDocumentModel>(repository);

            // act
            var results = await documentService.GetAllAsync().ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.GetAllAsync()).MustHaveHappenedOnceExactly();
            A.Equals(results, expectedResults);
        }

        [Fact]
        public async Task DocumentGetAllListReturnsNullWhenMissingRepository()
        {
            // arrange
            var repository = A.Dummy<ICosmosRepository<TestDocumentModel>>();
            IEnumerable<TestDocumentModel>? expectedResults = null;

            A.CallTo(() => repository.GetAllAsync()).Returns(expectedResults);

            var documentService = new DocumentService<TestDocumentModel>(repository);

            // act
            var results = await documentService.GetAllAsync().ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.GetAllAsync()).MustHaveHappenedOnceExactly();
            A.Equals(results, expectedResults);
        }
    }
}
