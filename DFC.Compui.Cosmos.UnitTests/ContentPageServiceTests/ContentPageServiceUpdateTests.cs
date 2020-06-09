using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Cosmos.UnitTests.Models;
using FakeItEasy;
using System;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Compui.Cosmos.UnitTests.ContentPageTests
{
    [Trait("Category", "Page Service Unit Tests")]
    public class ContentPageUpdateTests
    {
        [Fact]
        public void ContentPageUpdateReturnsSuccessWWhenContentPageReplaced()
        {
            // arrange
            var repository = A.Fake<ICosmosRepository<TestContentPageModel>>();
            var testContentPageModel = A.Fake<TestContentPageModel>();
            var expectedResult = A.Fake<TestContentPageModel>();

            A.CallTo(() => repository.GetAsync(A<Expression<Func<TestContentPageModel, bool>>>.Ignored)).Returns(expectedResult);

            var contentPageService = new ContentPageService<TestContentPageModel>(repository);

            // act
            var result = contentPageService.UpsertAsync(testContentPageModel).Result;

            // assert
            A.CallTo(() => repository.UpsertAsync(testContentPageModel)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public async Task ContentPageUpdateReturnsArgumentNullExceptionWhenNullIsUsed()
        {
            // arrange
            TestContentPageModel? testContentPageModel = null;
            var repository = A.Fake<ICosmosRepository<TestContentPageModel>>();
            var contentPageService = new ContentPageService<TestContentPageModel>(repository);

            // act
            var exceptionResult = await Assert.ThrowsAsync<ArgumentNullException>(async () => await contentPageService.UpsertAsync(testContentPageModel).ConfigureAwait(false)).ConfigureAwait(false);

            // assert
            Assert.Equal("Value cannot be null. (Parameter 'model')", exceptionResult.Message);
        }

        [Fact]
        public void ContentPageUpdateReturnsNullWWhenContentPageNotReplaced()
        {
            // arrange
            var repository = A.Fake<ICosmosRepository<TestContentPageModel>>();
            var testContentPageModel = A.Fake<TestContentPageModel>();
            var expectedResult = A.Dummy<TestContentPageModel>();

            A.CallTo(() => repository.UpsertAsync(testContentPageModel)).Returns(HttpStatusCode.BadRequest);

            var contentPageService = new ContentPageService<TestContentPageModel>(repository);

            // act
            var result = contentPageService.UpsertAsync(testContentPageModel).Result;

            // assert
            A.CallTo(() => repository.UpsertAsync(testContentPageModel)).MustHaveHappenedOnceExactly();
            A.CallTo(() => repository.GetAsync(A<Expression<Func<TestContentPageModel, bool>>>.Ignored)).MustNotHaveHappened();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public void ContentPageUpdateReturnsNullWhenMissingRepository()
        {
            // arrange
            var repository = A.Dummy<ICosmosRepository<TestContentPageModel>>();
            var testContentPageModel = A.Fake<TestContentPageModel>();
            TestContentPageModel? expectedResult = null;

            A.CallTo(() => repository.UpsertAsync(testContentPageModel)).Returns(HttpStatusCode.FailedDependency);

            var contentPageService = new ContentPageService<TestContentPageModel>(repository);

            // act
            var result = contentPageService.UpsertAsync(testContentPageModel).Result;

            // assert
            A.CallTo(() => repository.UpsertAsync(testContentPageModel)).MustHaveHappenedOnceExactly();
            A.CallTo(() => repository.GetAsync(A<Expression<Func<TestContentPageModel, bool>>>.Ignored)).MustNotHaveHappened();
            A.Equals(result, expectedResult);
        }
    }
}
