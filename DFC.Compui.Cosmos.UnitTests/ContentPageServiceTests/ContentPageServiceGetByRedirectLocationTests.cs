using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Cosmos.UnitTests.Models;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Compui.Cosmos.UnitTests.ContentPageTests
{
    [Trait("Category", "Content Page Service Unit Tests")]
    public class ContentPageServiceGetByRedirectLocationTests
    {
        [Fact]
        public void ContentPageGetByRedirectLocationReturnsSuccess()
        {
            // arrange
            const string redirectLocation = "name1";
            var repository = A.Fake<ICosmosRepository<TestContentPageModel>>();
            var expectedResult = A.CollectionOfFake<TestContentPageModel>(2);

            A.CallTo(() => repository.GetAsync(A<Expression<Func<TestContentPageModel, bool>>>.Ignored)).Returns(expectedResult);

            var contentPageService = new ContentPageService<TestContentPageModel>(repository);

            // act
            var result = contentPageService.GetByRedirectLocationAsync(redirectLocation).Result;

            // assert
            A.CallTo(() => repository.GetAsync(A<Expression<Func<TestContentPageModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult.First());
        }

        [Fact]
        public async Task ContentPageGetByRedirectLocationReturnsArgumentNullExceptionWhenNullNameIsUsed()
        {
            // arrange
            string? redirectLocation = null;
            var repository = A.Fake<ICosmosRepository<TestContentPageModel>>();
            var contentPageService = new ContentPageService<TestContentPageModel>(repository);

            // act
            var exceptionResult = await Assert.ThrowsAsync<ArgumentNullException>(async () => await contentPageService.GetByRedirectLocationAsync(redirectLocation).ConfigureAwait(false)).ConfigureAwait(false);

            // assert
            Assert.Equal("Value cannot be null. (Parameter 'redirectLocation')", exceptionResult.Message);
        }

        [Fact]
        public void ContentPageGetByRedirectLocationReturnsNullWhenMissingRepository()
        {
            // arrange
            const string redirectLocation = "name1";
            var repository = A.Dummy<ICosmosRepository<TestContentPageModel>>();
            IEnumerable<TestContentPageModel>? expectedResult = null;

            A.CallTo(() => repository.GetAsync(A<Expression<Func<TestContentPageModel, bool>>>.Ignored)).Returns(expectedResult);

            var contentPageService = new ContentPageService<TestContentPageModel>(repository);

            // act
            var result = contentPageService.GetByRedirectLocationAsync(redirectLocation).Result;

            // assert
            A.CallTo(() => repository.GetAsync(A<Expression<Func<TestContentPageModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }
    }
}
