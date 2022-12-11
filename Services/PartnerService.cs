using ChainStoreApi.Data;
using ChainStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ChainStoreApi.Services;

public class PartnerService
{
    private readonly IMongoCollection<Partner> _partnerCollection;
    public PartnerService(IOptions<DatabaseSetting> databaseSetting)
        {
            var mongoClient = new MongoClient(
                databaseSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(databaseSetting.Value.DatabaseName);

            _partnerCollection = mongoDatabase.GetCollection<Partner>(databaseSetting.Value.PartnerCollectionName);
        }
    public async Task<List<Partner>> GetPartnerAsync()=> await _partnerCollection.Find(_ => true).ToListAsync();
    public async Task<Partner?> GetPartnerAsync(string id) => await _partnerCollection.Find(x => x.id == id).FirstOrDefaultAsync();
    public async Task CreatePartnerAsync(Partner partner) => await _partnerCollection.InsertOneAsync(partner);
    public async Task UpdatePartnerAsync(string id, Partner partner) => await _partnerCollection.ReplaceOneAsync(x => x.id == id, partner);
    public async Task RemovePartnerAsync(string id) => await _partnerCollection.DeleteOneAsync(x=> x.id == id);
    
}