using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Cosmos.UnitTests.Models;
using FakeItEasy;
using System;
using Xunit;

namespace DFC.Compui.Cosmos.UnitTests.ContentPageTests
{
    [Trait("Category", "Page Service Unit Tests")]
    public class ContentPageGetByIdTests
    {
        [Fact]
        public void ContentPageGetByIdReturnsSuccess()
        {
            // arrange
            Guid documentId = Guid.NewGuid();
            var repository = A.Fake<ICosmosRepository<TestContentPageModel>>();
            var expectedResult = A.Fake<TestContentPageModel>();

            A.CallTo(() => repository.GetByIdAsync(A<Guid>.Ignored)).Returns(expectedResult);

            var contentPageService = new ContentPageService<TestContentPageModel>(repository);

            // act
            var result = contentPageService.GetByIdAsync(documentId).Result;

            // assert
            A.CallTo(() => repository.GetByIdAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public void ContentPageGetByIdReturnsNullWhenMissingRepository()
        {
            // arrange
            Guid documentId = Guid.NewGuid();
            var repository = A.Fake<ICosmosRepository<TestContentPageModel>>();
            TestContentPageModel? expectedResult = null;

            A.CallTo(() => repository.GetByIdAsync(A<Guid>.Ignored)).Returns(expectedResult);

            var contentPageService = new ContentPageService<TestContentPageModel>(repository);

            // act
            var result = contentPageService.GetByIdAsync(documentId).Result;

            // assert
            A.CallTo(() => repository.GetByIdAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }
    }
}
