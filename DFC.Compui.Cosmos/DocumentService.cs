using DFC.Compui.Cosmos.Contracts;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace DFC.Compui.Cosmos
{
    public class DocumentService<TModel> : IDocumentService<TModel>
            where TModel : class, IDocumentModel
    {
        public DocumentService(ICosmosRepository<TModel> repository)
        {
            Repository = repository;
        }

        protected ICosmosRepository<TModel> Repository { get; }

        public async Task<bool> PingAsync()
        {
            return await Repository.PingAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<TModel>?> GetAllAsync()
        {
            return await Repository.GetAllAsync().ConfigureAwait(false);
        }

        public async Task<TModel?> GetByIdAsync(Guid id)
        {
            return await Repository.GetByIdAsync(id).ConfigureAwait(false);
        }

        public async Task<TModel?> GetAsync(Expression<Func<TModel, bool>> where)
        {
            return await Repository.GetAsync(where).ConfigureAwait(false);
        }

        public async Task<HttpStatusCode> UpsertAsync(TModel? model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return await Repository.UpsertAsync(model).ConfigureAwait(false);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var result = await Repository.DeleteAsync(id).ConfigureAwait(false);

            return result == HttpStatusCode.NoContent;
        }
    }
}