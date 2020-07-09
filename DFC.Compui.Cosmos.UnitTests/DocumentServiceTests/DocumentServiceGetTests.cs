using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Cosmos.UnitTests.Models;
using FakeItEasy;
using System;
using System.Linq.Expressions;
using Xunit;

namespace DFC.Compui.Cosmos.UnitTests.DocumentTests
{
    [Trait("Category", "Document Service Unit Tests")]
    public class DocumentServiceGetTests
    {
        [Fact]
        public void DocumentGetReturnsSuccess()
        {
            // arrange
            const string contentValue = "some content";
            var repository = A.Fake<ICosmosRepository<TestDocumentModel>>();
            var expectedResult = A.Fake<TestDocumentModel>();

            A.CallTo(() => repository.GetAsync(A<Expression<Func<TestDocumentModel, bool>>>.Ignored)).Returns(expectedResult);

            var documentService = new DocumentService<TestDocumentModel>(repository);

            // act
            var result = documentService.GetAsync(d => d.Content == contentValue).Result;

            // assert
            A.CallTo(() => repository.GetAsync(A<Expression<Func<TestDocumentModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public void DocumentGetReturnsNullWhenMissingRepository()
        {
            // arrange
            const string contentValue = "some content";
            var repository = A.Fake<ICosmosRepository<TestDocumentModel>>();
            TestDocumentModel? expectedResult = null;

            A.CallTo(() => repository.GetAsync(A<Expression<Func<TestDocumentModel, bool>>>.Ignored)).Returns(expectedResult);

            var documentService = new DocumentService<TestDocumentModel>(repository);

            // act
            var result = documentService.GetAsync(d => d.Content == contentValue).Result;

            // assert
            A.CallTo(() => repository.GetAsync(A<Expression<Func<TestDocumentModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }
    }
}
