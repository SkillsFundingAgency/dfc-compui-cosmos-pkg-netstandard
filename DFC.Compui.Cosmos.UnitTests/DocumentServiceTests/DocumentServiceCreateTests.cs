using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Cosmos.UnitTests.Models;
using FakeItEasy;
using System;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Compui.Cosmos.UnitTests.DocumentTests
{
    [Trait("Category", "Document Service Unit Tests")]
    public class DocumentServiceCreateTests
    {
        [Fact]
        public void DocumentCreateReturnsSuccessWWhenDocumentCreated()
        {
            // arrange
            var repository = A.Fake<ICosmosRepository<TestDocumentModel>>();
            var testDocumentModel = A.Fake<TestDocumentModel>();
            var expectedResult = A.Fake<TestDocumentModel>();

            A.CallTo(() => repository.UpsertAsync(testDocumentModel)).Returns(HttpStatusCode.Created);

            var documentService = new DocumentService<TestDocumentModel>(repository);

            // act
            var result = documentService.UpsertAsync(testDocumentModel).Result;

            // assert
            A.CallTo(() => repository.UpsertAsync(testDocumentModel)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public async Task DocumentCreateReturnsArgumentNullExceptionWhenNullIsUsedAsync()
        {
            // arrange
            TestDocumentModel? testDocumentModel = null;
            var repository = A.Fake<ICosmosRepository<TestDocumentModel>>();
            var documentService = new DocumentService<TestDocumentModel>(repository);

            // act
            var exceptionResult = await Assert.ThrowsAsync<ArgumentNullException>(async () => await documentService.UpsertAsync(testDocumentModel).ConfigureAwait(false)).ConfigureAwait(false);

            // assert
            Assert.Equal("Value cannot be null. (Parameter 'model')", exceptionResult.Message);
        }

        [Fact]
        public void DocumentCreateReturnsNullWWhenDocumentNotCreated()
        {
            // arrange
            var repository = A.Fake<ICosmosRepository<TestDocumentModel>>();
            var testDocumentModel = A.Fake<TestDocumentModel>();
            var expectedResult = A.Dummy<TestDocumentModel>();

            A.CallTo(() => repository.UpsertAsync(testDocumentModel)).Returns(HttpStatusCode.BadRequest);

            var documentService = new DocumentService<TestDocumentModel>(repository);

            // act
            var result = documentService.UpsertAsync(testDocumentModel).Result;

            // assert
            A.CallTo(() => repository.UpsertAsync(testDocumentModel)).MustHaveHappenedOnceExactly();
            A.CallTo(() => repository.GetAsync(A<Expression<Func<TestDocumentModel, bool>>>.Ignored)).MustNotHaveHappened();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public void DocumentCreateReturnsNullWhenMissingRepository()
        {
            // arrange
            var repository = A.Dummy<ICosmosRepository<TestDocumentModel>>();
            var testDocumentModel = A.Fake<TestDocumentModel>();
            TestDocumentModel? expectedResult = null;

            A.CallTo(() => repository.UpsertAsync(testDocumentModel)).Returns(HttpStatusCode.FailedDependency);

            var documentService = new DocumentService<TestDocumentModel>(repository);

            // act
            var result = documentService.UpsertAsync(testDocumentModel).Result;

            // assert
            A.CallTo(() => repository.UpsertAsync(testDocumentModel)).MustHaveHappenedOnceExactly();
            A.CallTo(() => repository.GetAsync(A<Expression<Func<TestDocumentModel, bool>>>.Ignored)).MustNotHaveHappened();
            A.Equals(result, expectedResult);
        }
    }
}
