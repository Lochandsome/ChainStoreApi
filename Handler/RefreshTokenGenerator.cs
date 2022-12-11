namespace ChainStoreApi.Handler;
using ChainStoreApi.Data;
using ChainStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Security.Cryptography;
public class RefreshTokenGenerator
{
    private readonly IMongoCollection<RefreshToken> _refreshTokenCollection;

    public RefreshTokenGenerator(IOptions<DatabaseSetting> databaseSetting)
        {
            var mongoClient = new MongoClient(
                databaseSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(databaseSetting.Value.DatabaseName);

            _refreshTokenCollection = mongoDatabase.GetCollection<RefreshToken>(databaseSetting.Value.RefreshTokenCollectionName);
        }
    public async Task<string> GenerateToken(Account account)
    {
        var ramdomnumber = new byte[32];
        using(var randaomnumbergenerator = RandomNumberGenerator.Create())
        {
            randaomnumbergenerator.GetBytes(ramdomnumber);
            string refreshtoken = Convert.ToBase64String(ramdomnumber);
            var token = await _refreshTokenCollection.Find(x=> x.account == account).FirstOrDefaultAsync();
            if(token is not null)
            {
                RefreshToken refup = new RefreshToken(refreshtoken,account);
                await _refreshTokenCollection.ReplaceOneAsync(x=>x.account==account,refup);
                
            }
            else
            { 
                RefreshToken refup = new RefreshToken(refreshtoken,account);
                await _refreshTokenCollection.InsertOneAsync(refup);
            }
            return refreshtoken;
        }
    }
}