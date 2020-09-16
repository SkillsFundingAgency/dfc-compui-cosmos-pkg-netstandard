using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Cosmos.UnitTests.Models;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace DFC.Compui.Cosmos.UnitTests.DocumentTests
{
    [Trait("Category", "Document Service Unit Tests")]
    public class DocumentServiceGetTests
    {
        [Fact]
        public void DocumentGetByPartitionKeyReturnsSuccess()
        {
            // arrange
            const string contentValue = "some content";
            const string partitionKeyValue = "a partition key";
            var repository = A.Fake<ICosmosRepository<TestDocumentModel>>();
            var expectedResult = A.Fake<TestDocumentModel>();

            A.CallTo(() => repository.GetAsync(A<Expression<Func<TestDocumentModel, bool>>>.Ignored, A<string>.Ignored)).Returns(expectedResult);

            var documentService = new DocumentService<TestDocumentModel>(repository);

            // act
            var result = documentService.GetAsync(d => d.Content == contentValue, partitionKeyValue).Result;

            // assert
            A.CallTo(() => repository.GetAsync(A<Expression<Func<TestDocumentModel, bool>>>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public void DocumentGetReturnsSuccess()
        {
            // arrange
            const string contentValue = "some content";
            var repository = A.Fake<ICosmosRepository<TestDocumentModel>>();
            var expectedResult = A.CollectionOfFake<TestDocumentModel>(2);

            A.CallTo(() => repository.GetAsync(A<Expression<Func<TestDocumentModel, bool>>>.Ignored)).Returns(expectedResult);

            var documentService = new DocumentService<TestDocumentModel>(repository);

            // act
            var result = documentService.GetAsync(d => d.Content == contentValue).Result;

            // assert
            A.CallTo(() => repository.GetAsync(A<Expression<Func<TestDocumentModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult.First());
        }

        [Fact]
        public void DocumentGetReturnsNullWhenMissingRepository()
        {
            // arrange
            const string contentValue = "some content";
            var repository = A.Fake<ICosmosRepository<TestDocumentModel>>();
            IEnumerable<TestDocumentModel>? expectedResult = null;

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
