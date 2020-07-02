using System.Threading.Tasks;

namespace DFC.Compui.Cosmos.Contracts
{
    public interface IContentPageService<TModel> : IDocumentService<TModel>
        where TModel : class, IContentPageModel
    {
        Task<TModel?> GetByNameAsync(string? pagelocation, string? canonicalName);

        Task<TModel?> GetByRedirectLocationAsync(string? redirectLocation);
    }
}