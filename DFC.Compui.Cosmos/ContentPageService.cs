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

        public async Task<TModel?> GetByNameAsync(string? pagelocation, string? canonicalName)
        {
            if (string.IsNullOrWhiteSpace(pagelocation))
            {
                throw new ArgumentNullException(nameof(pagelocation));
            }

            if (string.IsNullOrWhiteSpace(canonicalName))
            {
                throw new ArgumentNullException(nameof(canonicalName));
            }

            return await Repository.GetAsync(d => d.Pagelocation == pagelocation.ToLowerInvariant() && d.CanonicalName == canonicalName.ToLowerInvariant()).ConfigureAwait(false);
        }

        public async Task<TModel?> GetByRedirectLocationAsync(string? redirectLocation)
        {
            if (string.IsNullOrWhiteSpace(redirectLocation))
            {
                throw new ArgumentNullException(nameof(redirectLocation));
            }

            return await Repository.GetAsync(d => d.RedirectLocations!.Contains(redirectLocation.ToLowerInvariant())).ConfigureAwait(false);
        }
    }
}