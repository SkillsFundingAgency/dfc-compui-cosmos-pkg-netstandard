﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace DFC.Compui.Cosmos.Contracts
{
    public interface IContentPageModel : IDocumentModel
    {
        string? CanonicalName { get; set; }

        public IList<string>? AlternativeNames { get; set; }
    }
}