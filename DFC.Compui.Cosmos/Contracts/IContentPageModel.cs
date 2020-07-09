using System;
using System.Collections.Generic;

namespace DFC.Compui.Cosmos.Contracts
{
    public interface IContentPageModel : IDocumentModel
    {
        string? CanonicalName { get; set; }

        string? PageLocation { get; set; }

        IList<string>? RedirectLocations { get; set; }

        Guid? Version { get; set; }
    }
}
