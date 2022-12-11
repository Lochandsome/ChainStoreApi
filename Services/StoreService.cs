using ChainStoreApi.Data;
using ChainStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ChainStoreApi.Services;

public class StoreService
{
    private readonly IMongoCollection<Store> _storeService;

    public StoreService(
        IOptions<DatabaseSetting> databaseSetting)
        {
            var mongoClient = new MongoClient(
                databaseSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(databaseSetting.Value.DatabaseName);

            _storeService = mongoDatabase.GetCollection<Store>(databaseSetting.Value.StoreCollectionName);
        }
    public async Task<List<Store>> GetStoreAsync()=> await _storeService.Find(_ => true).ToListAsync();
    public async Task<Store?> GetStoreAsync(string id) => await _storeService.Find(x => x.id == id).FirstOrDefaultAsync();
    public async Task CreateStoreAsync(Store store) => await _storeService.InsertOneAsync(store);
    public async Task UpdateStoreAsync(string id, Store store) => await _storeService.ReplaceOneAsync(x => x.id == id, store);
    public async Task RemoveStoreAsync(string id) => await _storeService.DeleteOneAsync(x=> x.id == id);
}