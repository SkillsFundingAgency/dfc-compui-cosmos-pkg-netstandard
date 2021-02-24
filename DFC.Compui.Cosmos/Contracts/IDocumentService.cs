using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace DFC.Compui.Cosmos.Contracts
{
    public interface IDocumentService<TModel>
        where TModel : class, IDocumentModel
    {
        Task<bool> PingAsync();

        Task<IEnumerable<TModel>?> GetAllAsync(string? partitionKeyValue = null);

        Task<TModel?> GetByIdAsync(Guid id, string? partitionKey = null);

        Task<IEnumerable<TModel>?> GetAsync(Expression<Func<TModel, bool>> where);

        Task<TModel?> GetAsync(Expression<Func<TModel, bool>> where, string partitionKeyValue);

        Task<HttpStatusCode> UpsertAsync(TModel model);

        Task<bool> DeleteAsync(Guid id);

        Task<bool> PurgeAsync();
    }
}