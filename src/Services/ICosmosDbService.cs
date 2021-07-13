namespace NotesKeeper
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NotesKeeper.Models;

    public interface ICosmosDbService
    {
        Task<IEnumerable<Item>> GetItemsAsync(string query);
        Task<Item> GetItemAsync(string upn, string id);
        Task AddItemAsync(Item item);
        Task UpdateItemAsync(Item item);
        Task DeleteItemAsync(string upn, string id);
    }
}
