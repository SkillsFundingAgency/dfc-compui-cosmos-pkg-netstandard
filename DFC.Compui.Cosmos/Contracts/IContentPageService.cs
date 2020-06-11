using System.Threading.Tasks;

namespace DFC.Compui.Cosmos.Contracts
{
    public interface IContentPageService<TModel> : IDocumentService<TModel>
        where TModel : class, IContentPageModel
    {
        Task<TModel?> GetByNameAsync(string? canonicalName);

        Task<TModel?> GetByAlternativeNameAsync(string? alternativeName);
    }
}