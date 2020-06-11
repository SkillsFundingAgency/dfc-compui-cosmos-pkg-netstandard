using DFC.Compui.Cosmos.Contracts;
using System;
using System.Threading.Tasks;

namespace DFC.Compui.Cosmos
{
    public class ContentPageService<TModel> : DocumentService<TModel>, IContentPageService<TModel>
            where TModel : class, IContentPageModel
    {
        public ContentPageService(ICosmosRepository<TModel> repository) : base(repository)
        {
        }

        public async Task<TModel?> GetByNameAsync(string? canonicalName)
        {
            if (string.IsNullOrWhiteSpace(canonicalName))
            {
                throw new ArgumentNullException(nameof(canonicalName));
            }

            return await Repository.GetAsync(d => d.CanonicalName == canonicalName.ToLowerInvariant()).ConfigureAwait(false);
        }

        public async Task<TModel?> GetByAlternativeNameAsync(string? alternativeName)
        {
            if (string.IsNullOrWhiteSpace(alternativeName))
            {
                throw new ArgumentNullException(nameof(alternativeName));
            }

            return await Repository.GetAsync(d => d.AlternativeNames!.Contains(alternativeName.ToLowerInvariant())).ConfigureAwait(false);
        }
    }
}