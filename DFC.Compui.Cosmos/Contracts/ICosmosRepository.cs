using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace DFC.Compui.Cosmos.Contracts
{
    public interface ICosmosRepository<TModel>
        where TModel : class, IDocumentModel
    {
        Task<bool> PingAsync();

        Task<TModel?> GetByIdAsync(Guid id, string? partitionKeyValue = null);

        Task<IEnumerable<TModel>?> GetAsync(Expression<Func<TModel, bool>> where);

        Task<TModel?> GetAsync(Expression<Func<TModel, bool>> where, string partitionKeyValue);

        Task<IEnumerable<TModel>?> GetAllAsync(string? partitionKeyValue = null);

        Task<HttpStatusCode> UpsertAsync(TModel model);

        Task<HttpStatusCode> DeleteAsync(Guid id);

        Task<HttpStatusCode> PurgeAsync();
    }
}