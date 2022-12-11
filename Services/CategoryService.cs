using ChainStoreApi.Data;
using ChainStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ChainStoreApi.Services;

public class CategoryService
{
    private readonly IMongoCollection<Category> _categoryCollection;
    public CategoryService(IOptions<DatabaseSetting> databaseSetting)
        {
            var mongoClient = new MongoClient(
                databaseSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(databaseSetting.Value.DatabaseName);

            _categoryCollection = mongoDatabase.GetCollection<Category>(databaseSetting.Value.CategoryCollectionName);
        }
    public async Task<List<Category>> GetCategoryAsync()=> await _categoryCollection.Find(_ => true).ToListAsync();
    public async Task<Category?> GetCategoryAsync(string id) => await _categoryCollection.Find(x => x.id == id).FirstOrDefaultAsync();
    public async Task CreateCategoryAsync(Category category) => await _categoryCollection.InsertOneAsync(category);
    public async Task UpdateCategoryAsync(string id, Category category) => await _categoryCollection.ReplaceOneAsync(x => x.id == id, category);
    public async Task RemoveCategoryAsync(string id) => await _categoryCollection.DeleteOneAsync(x=> x.id == id);
    
}