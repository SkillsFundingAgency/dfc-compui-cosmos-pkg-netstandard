using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Cosmos.UnitTests.Models;
using FakeItEasy;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Compui.Cosmos.UnitTests.ContentPageTests
{
    [Trait("Category", "Page Service Unit Tests")]
    public class ContentPageServiceGetAllTests
    {
        [Fact]
        public async Task ContentPageGetAllListReturnsSuccess()
        {
            // arrange
            var repository = A.Fake<ICosmosRepository<TestContentPageModel>>();
            var expectedResults = A.CollectionOfFake<TestContentPageModel>(2);

            A.CallTo(() => repository.GetAllAsync()).Returns(expectedResults);

            var contentPageService = new ContentPageService<TestContentPageModel>(repository);

            // act
            var results = await contentPageService.GetAllAsync().ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.GetAllAsync()).MustHaveHappenedOnceExactly();
            A.Equals(results, expectedResults);
        }

        [Fact]
        public async Task ContentPageGetAllListReturnsNullWhenMissingRepository()
        {
            // arrange
            var repository = A.Dummy<ICosmosRepository<TestContentPageModel>>();
            IEnumerable<TestContentPageModel>? expectedResults = null;

            A.CallTo(() => repository.GetAllAsync()).Returns(expectedResults);

            var contentPageService = new ContentPageService<TestContentPageModel>(repository);

            // act
            var results = await contentPageService.GetAllAsync().ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.GetAllAsync()).MustHaveHappenedOnceExactly();
            A.Equals(results, expectedResults);
        }
    }
}
