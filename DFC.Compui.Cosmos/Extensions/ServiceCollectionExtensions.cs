using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Telemetry.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;

namespace DFC.Compui.Cosmos
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDocumentServices<TModel>(this IServiceCollection services, CosmosDbConnection cosmosDbConnection, bool isDevelopment)
         where TModel : class, IDocumentModel
        {
            _ = cosmosDbConnection ?? throw new ArgumentNullException(nameof(cosmosDbConnection));

            var documentClient = new DocumentClient(cosmosDbConnection!.EndpointUrl, cosmosDbConnection!.AccessKey);
            object[] serviceArguments = { cosmosDbConnection, documentClient, isDevelopment };

            services.AddSingleton(cosmosDbConnection);
            services.AddSingleton<IDocumentClient>(documentClient);
            services.AddSingleton<ICosmosRepository<TModel>>(x => ActivatorUtilities.CreateInstance<CosmosRepository<TModel>>(x, serviceArguments));
            services.AddTransient<IDocumentService<TModel>, DocumentService<TModel>>();

            return services;
        }

        public static IServiceCollection AddContentPageServices<TModel>(this IServiceCollection services, CosmosDbConnection cosmosDbConnection, bool isDevelopment)
         where TModel : RequestTrace, IContentPageModel
        {
            _ = cosmosDbConnection ?? throw new ArgumentNullException(nameof(cosmosDbConnection));

            var documentClient = new DocumentClient(cosmosDbConnection!.EndpointUrl, cosmosDbConnection!.AccessKey);
            object[] serviceArguments = { cosmosDbConnection, documentClient, isDevelopment };

            services.AddSingleton(cosmosDbConnection);
            services.AddSingleton<IDocumentClient>(documentClient);
            services.AddSingleton<ICosmosRepository<TModel>>(x => ActivatorUtilities.CreateInstance<CosmosRepository<TModel>>(x, serviceArguments));
            services.AddTransient<IContentPageService<TModel>, ContentPageService<TModel>>();

            return services;
        }
    }
}
