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

        Task<IEnumerable<TModel>?> GetAllAsync();

        Task<TModel?> GetByIdAsync(Guid id);

        Task<IEnumerable<TModel>?> GetAsync(Expression<Func<TModel, bool>> where);

        Task<HttpStatusCode> UpsertAsync(TModel model);

        Task<bool> DeleteAsync(Guid id);
    }
}