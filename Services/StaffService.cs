using ChainStoreApi.Data;
using ChainStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections.Generic;


namespace ChainStoreApi.Services;

    public class StaffService
    {
        private readonly IMongoCollection<Staff> _staffCollection;
        public StaffService(
            IOptions<DatabaseSetting> databaseSetting)
        {
            var mongoClient = new MongoClient(
                databaseSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(databaseSetting.Value.DatabaseName);

            _staffCollection = mongoDatabase.GetCollection<Staff>(databaseSetting.Value.StaffCollectionName);
        }

        public async Task<Staff?> GetStaffLogin(string accountId) => await _staffCollection.Find(x=>x.accountId == accountId).FirstOrDefaultAsync();
        public async Task<List<Staff>> GetStaffAsyncStore(string idstore)=> await _staffCollection.Find(x => x.storeId == idstore).ToListAsync();
        public async Task<Staff?> GetStaffAsync(string id) => await _staffCollection.Find(x => x.id == id).FirstOrDefaultAsync();
        public async Task<Staff?> GetStaffWithAccountIdAsync(string accountid) => await _staffCollection.Find(x => x.accountId == accountid).FirstOrDefaultAsync();
        
        public async Task CreateStaffAsync(Staff staff) => await _staffCollection.InsertOneAsync(staff);
        
        public async Task UpdateStaffAsync(string id, Staff staff) => await _staffCollection.ReplaceOneAsync(x => x.id == id, staff);
        public async Task RemoveStaffAsync(string id) => await _staffCollection.DeleteOneAsync(x=> x.id == id);

        public async Task<List<Staff>> GetStaffInStoreAsync(string storeID) => await _staffCollection.Find(x => x.storeId == storeID).ToListAsync();

        public async Task<Object> GetSearchStaff(string? nameStaff, string? nameStore, int? page){
            
             var filterStaff = Builders<Staff>.Filter.Empty;
             var filterStore = Builders<Store>.Filter.Empty;
             
            if(string.IsNullOrEmpty(nameStaff) && string.IsNullOrEmpty(nameStore))
            {
                
                var data = await _staffCollection.Find(_ =>true).Skip(page*10 - 10).Limit(10).ToListAsync();
                var total =  await  _staffCollection.CountDocumentsAsync(filterStaff);
                return new
                {
                    data,
                    total
                };
            }
            else
            {
                filterStore = Builders<Store>.Filter.Regex("namestore",new BsonRegularExpression(nameStore,"i"));
                filterStaff = Builders<Staff>.Filter.Regex("fullname", new BsonRegularExpression(nameStaff, "i"));

                // search with nameStore 
                if(string.IsNullOrEmpty(nameStaff))
                {
                    
                }
                // search with nameStaff
                if(string.IsNullOrEmpty(nameStore))
                {
                    var data = await _staffCollection.Find(filterStaff).Skip(page*10 - 10).Limit(10).ToListAsync();
                    var total =  await  _staffCollection.CountDocumentsAsync(filterStaff);
                    return new
                    {
                        data,
                        total
                    };
                }
            }
            return await _staffCollection.Find(_=>true).Skip(page*10 - 10).Limit(10).ToListAsync();

        }

    }
