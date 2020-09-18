using DFC.Compui.Telemetry.Interface;
using System;

namespace DFC.Compui.Cosmos.Contracts
{
    public interface IDocumentModel : IRequestTrace
    {
        Guid Id { get; set; }

        string? Etag { get; set; }

        string? PartitionKey { get; set; }
    }
}
