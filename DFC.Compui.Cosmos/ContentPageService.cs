using DFC.Compui.Cosmos.Contracts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.Compui.Cosmos
{
    public class ContentPageService<TModel> : DocumentService<TModel>, IContentPageService<TModel>
            where TModel : class, IContentPageModel
    {
        public ContentPageService(ICosmosRepository<TModel> repository) : base(repository)
        {
        }

        public async Task<TModel?> GetByNameAsync(string? pageLocation, string? canonicalName)
        {
            if (string.IsNullOrWhiteSpace(pageLocation))
            {
                throw new ArgumentNullException(nameof(pageLocation));
            }

            if (string.IsNullOrWhiteSpace(canonicalName))
            {
                throw new ArgumentNullException(nameof(canonicalName));
            }

            var model = await Repository.GetAsync(d => d.CanonicalName == canonicalName.ToLowerInvariant(), pageLocation.ToLowerInvariant()).ConfigureAwait(false);

            return model;
        }

        public async Task<TModel?> GetByNameAsync(string? canonicalName)
        {
            if (string.IsNullOrWhiteSpace(canonicalName))
            {
                throw new ArgumentNullException(nameof(canonicalName));
            }

            var models = await Repository.GetAsync(d => d.CanonicalName == canonicalName.ToLowerInvariant()).ConfigureAwait(false);

            if (models != null && models.Any())
            {
                return models.FirstOrDefault();
            }

            return default;
        }

        public async Task<TModel?> GetByRedirectLocationAsync(string? redirectLocation)
        {
            if (string.IsNullOrWhiteSpace(redirectLocation))
            {
                throw new ArgumentNullException(nameof(redirectLocation));
            }

            var models = await Repository.GetAsync(d => d.RedirectLocations!.Contains(redirectLocation.ToLowerInvariant())).ConfigureAwait(false);

            if (models != null && models.Any())
            {
                return models.FirstOrDefault();
            }

            return default;
        }
    }
}