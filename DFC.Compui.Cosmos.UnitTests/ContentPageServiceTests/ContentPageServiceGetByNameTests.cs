using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Cosmos.UnitTests.Models;
using FakeItEasy;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Compui.Cosmos.UnitTests.ContentPageTests
{
    [Trait("Category", "Page Service Unit Tests")]
    public class ContentPageServiceGetByNameTests
    {
        private const string CanonicalName = "name1";
        private readonly ICosmosRepository<TestContentPageModel> repository;
        private readonly IContentPageService<TestContentPageModel> contentPageService;

        public ContentPageServiceGetByNameTests()
        {
            repository = A.Fake<ICosmosRepository<TestContentPageModel>>();
            contentPageService = new ContentPageService<TestContentPageModel>(repository);
        }

        [Fact]
        public async Task ContentPageGetByNameReturnsSuccess()
        {
            // arrange
            var expectedResult = A.Fake<TestContentPageModel>();
            A.CallTo(() => repository.GetAsync(A<Expression<Func<TestContentPageModel, bool>>>.Ignored)).Returns(expectedResult);

            // act
            var result = await contentPageService.GetByNameAsync(CanonicalName).ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.GetAsync(A<Expression<Func<TestContentPageModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public async Task ContentPageGetByNameReturnsArgumentNullExceptionWhenNullNameIsUsed()
        {
            // arrange
            string? canonicalName = null;

            // act
            var exceptionResult = await Assert.ThrowsAsync<ArgumentNullException>(async () => await contentPageService.GetByNameAsync(canonicalName).ConfigureAwait(false)).ConfigureAwait(false);

            // assert
            Assert.Equal("Value cannot be null. (Parameter 'canonicalName')", exceptionResult.Message);
        }

        [Fact]
        public async Task ContentPageGetByNameReturnsNullWhenMissingRepository()
        {
            // arrange
            TestContentPageModel? expectedResult = null;

            A.CallTo(() => repository.GetAsync(A<Expression<Func<TestContentPageModel, bool>>>.Ignored)).Returns(expectedResult);

            // act
            var result = await contentPageService.GetByNameAsync(CanonicalName).ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.GetAsync(A<Expression<Func<TestContentPageModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Null(result);
        }
    }
}