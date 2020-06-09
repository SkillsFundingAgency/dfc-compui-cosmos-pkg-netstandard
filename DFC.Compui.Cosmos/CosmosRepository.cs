using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace DFC.Compui.Cosmos.Contracts
{
    [ExcludeFromCodeCoverage]
    public class CosmosRepository<TModel> : ICosmosRepository<TModel>
        where TModel : class, IDocumentModel
    {
        private readonly CosmosDbConnection cosmosDbConnection;
        private readonly IDocumentClient documentClient;

        public CosmosRepository(CosmosDbConnection cosmosDbConnection, IDocumentClient documentClient, bool isDevelopment)
        {
            this.cosmosDbConnection = cosmosDbConnection;
            this.documentClient = documentClient;

            if (isDevelopment)
            {
                InitialiseDevEnvironment().ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }

        private Uri DocumentCollectionUri => UriFactory.CreateDocumentCollectionUri(cosmosDbConnection.DatabaseId, cosmosDbConnection.CollectionId);

        public async Task InitialiseDevEnvironment()
        {
            await CreateDatabaseIfNotExistsAsync().ConfigureAwait(false);
            await CreateCollectionIfNotExistsAsync().ConfigureAwait(false);
        }

        public async Task<bool> PingAsync()
        {
            var query = documentClient.CreateDocumentQuery<TModel>(DocumentCollectionUri, new FeedOptions { MaxItemCount = 1, EnableCrossPartitionQuery = true })
                                       .AsDocumentQuery();

            if (query == null)
            {
                return false;
            }

            var models = await query.ExecuteNextAsync<TModel>().ConfigureAwait(false);
            var firstModel = models.FirstOrDefault();

            return firstModel != null;
        }

        public async Task<TModel?> GetByIdAsync(Guid id)
        {
            var query = documentClient.CreateDocumentQuery<TModel>(
                DocumentCollectionUri,
                new SqlQuerySpec($"SELECT * FROM c WHERE c.id = @id")
                {
                    Parameters = new SqlParameterCollection(new[]
                    {
                        new SqlParameter("@id", id),
                    }),
                },
                new FeedOptions { MaxItemCount = 1, EnableCrossPartitionQuery = true })
                .AsDocumentQuery();

            if (query == null)
            {
                return default;
            }

            var models = await query.ExecuteNextAsync<TModel>().ConfigureAwait(false);

            if (models != null && models.Count > 0)
            {
                return models.FirstOrDefault();
            }

            return default;
        }

        public async Task<TModel?> GetAsync(Expression<Func<TModel, bool>> where)
        {
            var query = documentClient.CreateDocumentQuery<TModel>(DocumentCollectionUri, new FeedOptions { MaxItemCount = 1, EnableCrossPartitionQuery = true })
                                      .Where(where)
                                      .AsDocumentQuery();

            if (query == null)
            {
                return default;
            }

            var models = await query.ExecuteNextAsync<TModel>().ConfigureAwait(false);

            if (models != null && models.Count > 0)
            {
                return models.FirstOrDefault();
            }

            return default;
        }

        public async Task<TModel?> GetAsync(string partitionKeyValue, Expression<Func<TModel, bool>> where)
        {
            var partitionKey = new PartitionKey(partitionKeyValue.ToLowerInvariant());

            var query = documentClient.CreateDocumentQuery<TModel>(DocumentCollectionUri, new FeedOptions { MaxItemCount = 1, PartitionKey = partitionKey })
                                      .Where(where)
                                      .AsDocumentQuery();

            if (query == null)
            {
                return default;
            }

            var models = await query.ExecuteNextAsync<TModel>().ConfigureAwait(false);

            if (models != null && models.Count > 0)
            {
                return models.FirstOrDefault();
            }

            return default;
        }

        public async Task<IEnumerable<TModel>?> GetAllAsync()
        {
            var query = documentClient.CreateDocumentQuery<TModel>(DocumentCollectionUri, new FeedOptions { EnableCrossPartitionQuery = true })
                                      .AsDocumentQuery();

            var models = new List<TModel>();

            while (query.HasMoreResults)
            {
                var result = await query.ExecuteNextAsync<TModel>().ConfigureAwait(false);

                models.AddRange(result);
            }

            return models.Any() ? models : default;
        }

        public async Task<IEnumerable<TModel>?> GetAllAsync(string partitionKeyValue)
        {
            var partitionKey = new PartitionKey(partitionKeyValue.ToLowerInvariant());

            var query = documentClient.CreateDocumentQuery<TModel>(DocumentCollectionUri, new FeedOptions { PartitionKey = partitionKey })
                                      .AsDocumentQuery();

            var models = new List<TModel>();

            while (query.HasMoreResults)
            {
                var result = await query.ExecuteNextAsync<TModel>().ConfigureAwait(false);

                models.AddRange(result);
            }

            return models.Any() ? models : default;
        }

        public async Task<HttpStatusCode> UpsertAsync(TModel model)
        {
            var accessCondition = new AccessCondition { Condition = model.Etag, Type = AccessConditionType.IfMatch };
            var partitionKey = new PartitionKey(model.PartitionKey);

            var result = await documentClient.UpsertDocumentAsync(DocumentCollectionUri, model, new RequestOptions { AccessCondition = accessCondition, PartitionKey = partitionKey }).ConfigureAwait(false);

            return result.StatusCode;
        }

        public async Task<HttpStatusCode> DeleteAsync(Guid id)
        {
            var documentUri = CreateDocumentUri(id);

            var model = await GetByIdAsync(id).ConfigureAwait(false);

            if (model != null)
            {
                var accessCondition = new AccessCondition { Condition = model.Etag, Type = AccessConditionType.IfMatch };
                var partitionKey = new PartitionKey(model.PartitionKey);

                var result = await documentClient.DeleteDocumentAsync(documentUri, new RequestOptions { AccessCondition = accessCondition, PartitionKey = partitionKey }).ConfigureAwait(false);

                return result.StatusCode;
            }

            return HttpStatusCode.NotFound;
        }

        private async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await documentClient.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(cosmosDbConnection.DatabaseId)).ConfigureAwait(false);
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await documentClient.CreateDatabaseAsync(new Database { Id = cosmosDbConnection.DatabaseId }).ConfigureAwait(false);
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                await documentClient.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(cosmosDbConnection.DatabaseId, cosmosDbConnection.CollectionId)).ConfigureAwait(false);
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    var partitionKeyDefinition = new PartitionKeyDefinition
                    {
                        Paths = new Collection<string>() { cosmosDbConnection.PartitionKey! },
                    };

                    await documentClient.CreateDocumentCollectionAsync(
                                UriFactory.CreateDatabaseUri(cosmosDbConnection.DatabaseId),
                                new DocumentCollection { Id = cosmosDbConnection.CollectionId, PartitionKey = partitionKeyDefinition },
                                new RequestOptions { OfferThroughput = 1000 }).ConfigureAwait(false);
                }
                else
                {
                    throw;
                }
            }
        }

        private Uri CreateDocumentUri(Guid id)
        {
            return UriFactory.CreateDocumentUri(cosmosDbConnection.DatabaseId, cosmosDbConnection.CollectionId, id.ToString());
        }
    }
}
