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

        Task<TModel?> GetByIdAsync(Guid id);

        Task<IEnumerable<TModel>?> GetAsync(Expression<Func<TModel, bool>> where);

        Task<TModel?> GetAsync(string? partitionKeyValue, Expression<Func<TModel, bool>> where);

        Task<IEnumerable<TModel>?> GetAllAsync();

        Task<IEnumerable<TModel>?> GetAllAsync(string? partitionKeyValue);

        Task<HttpStatusCode> UpsertAsync(TModel model);

        Task<HttpStatusCode> DeleteAsync(Guid id);
    }
}