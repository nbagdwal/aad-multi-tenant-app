using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NotesKeeper.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;
using NotesKeeper.Services;

namespace NotesKeeper
{
        public class CosmosDbService : ICosmosDbService, IAsyncInitialization
    {
        private Container _container;

        public CosmosDbService(IConfigurationSection configurationSection)
        {
            Initialization = InitializeAsync(configurationSection);
        }

        public Task Initialization { get; private set; }

        private async Task InitializeAsync(IConfigurationSection configurationSection)
        {
            // Asynchronously initialize this instance.
            string databaseName = configurationSection.GetSection("DatabaseName").Value;
            string containerName = configurationSection.GetSection("ContainerName").Value;
            string account = configurationSection.GetSection("Account").Value;
            string key = configurationSection.GetSection("Key").Value;

            CosmosClient client = new CosmosClient(account, key);
            DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(databaseName).ConfigureAwait(false);
            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/upn").ConfigureAwait(false);

            _container = client.GetContainer(databaseName, containerName);
        }

        public async Task AddItemAsync(Item item)
        {
            await _container.CreateItemAsync<Item>(item, new PartitionKey(item.UPN)).ConfigureAwait(false);
        }

        public async Task DeleteItemAsync(string upn, string id)
        {
            await _container.DeleteItemAsync<Item>(id, new PartitionKey(upn));
        }

        public async Task<Item> GetItemAsync(string upn, string id)
        {
            try
            {
                ItemResponse<Item> response = await _container.ReadItemAsync<Item>(id, new PartitionKey(upn)).ConfigureAwait(false);
                return response.Resource;
            }
            catch(CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            { 
                return null;
            }

        }

        public async Task<IEnumerable<Item>> GetItemsAsync(string queryString)
        {
            await Initialization.ConfigureAwait(false);
            var query = _container.GetItemQueryIterator<Item>(new QueryDefinition(queryString));
            List<Item> results = new List<Item>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync().ConfigureAwait(false);
                
                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdateItemAsync( Item item)
        {
            await _container.UpsertItemAsync<Item>(item, new PartitionKey(item.UPN)).ConfigureAwait(false);
        }
    }
}
