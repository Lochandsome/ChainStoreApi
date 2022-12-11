using ChainStoreApi.Data;
using ChainStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ChainStoreApi.Services;

public class ProductService
{
    private readonly IMongoCollection<Product> _productCollection;
    public ProductService(IOptions<DatabaseSetting> databaseSetting)
        {
            var mongoClient = new MongoClient(
                databaseSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(databaseSetting.Value.DatabaseName);

            _productCollection = mongoDatabase.GetCollection<Product>(databaseSetting.Value.ProductCollectionName);
        }
    public async Task<List<Product>> GetProductAsync()=> await _productCollection.Find(_ => true).ToListAsync();
    public async Task<Product?> GetProductAsync(string id) => await _productCollection.Find(x => x.id == id).FirstOrDefaultAsync();
    public async Task CreateProductAsync(Product product) => await _productCollection.InsertOneAsync(product);
    public async Task UpdateProductAsync(string id, Product product) => await _productCollection.ReplaceOneAsync(x => x.id == id, product);
    public async Task RemoveProductAsync(string id) => await _productCollection.DeleteOneAsync(x=> x.id == id);
    
    public async Task<Object> GetProductsAsyncWithCategory(string idCategory, int? page)
    { 
        
        var count = await _productCollection.CountDocumentsAsync(x=>x.category.id == idCategory);
        var data =  await _productCollection.Find(x => x.category.id == idCategory).Skip((page-1)*10).Limit(10).ToListAsync();
        
        return new {data, count, page};

        
    }
}