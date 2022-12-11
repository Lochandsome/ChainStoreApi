using ChainStoreApi.Data;
using ChainStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections.Generic;


namespace ChainStoreApi.Services;

    public class CustomerService
    {
        private readonly IMongoCollection<Customer> _customerCollection;
        public CustomerService(
            IOptions<DatabaseSetting> databaseSetting)
        {
            var mongoClient = new MongoClient(
                databaseSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(databaseSetting.Value.DatabaseName);

            _customerCollection = mongoDatabase.GetCollection<Customer>(databaseSetting.Value.CustomerCollectionName);
        }

        public async Task<List<Customer>> GetCustomerAsync() => await _customerCollection.Find(_ => true).ToListAsync();
        public async Task<Customer?> GetCustomerAsync(string id) => await _customerCollection.Find(x => x.id == id).FirstOrDefaultAsync();
        
        public async Task CreateCustomerAsync(Customer customer) => await _customerCollection.InsertOneAsync(customer);
        
        public async Task UpdateCustomerAsync(string id, Customer customer) => await _customerCollection.ReplaceOneAsync(x => x.id == id, customer);
        public async Task RemoveCustomerAsync(string id) => await _customerCollection.DeleteOneAsync(x=> x.id == id);

        public async Task<Object> GetSearchCustomer(string? nameCustomer, int? page){
            
             var filterCustomer = Builders<Customer>.Filter.Empty;
             
            if(string.IsNullOrEmpty(nameCustomer) && string.IsNullOrEmpty(nameCustomer))
            {
                
                var data = await _customerCollection.Find(_ =>true).Skip(page*10 - 10).Limit(10).ToListAsync();
                var total =  await  _customerCollection.CountDocumentsAsync(filterCustomer);
                return new
                {
                    data,
                    total
                };
            }
            else
            {
                filterCustomer = Builders<Customer>.Filter.Regex("namecustomer",new BsonRegularExpression(nameCustomer,"i"));

                // search with nameStore 
                if(string.IsNullOrEmpty(nameCustomer))
                {
                    
                }
                // search with nameStaff
                if(string.IsNullOrEmpty(nameCustomer))
                {
                    var data = await _customerCollection.Find(filterCustomer).Skip(page*10 - 10).Limit(10).ToListAsync();
                    var total =  await  _customerCollection.CountDocumentsAsync(filterCustomer);
                    return new
                    {
                        data,
                        total
                    };
                }
            }
            return await _customerCollection.Find(_=>true).Skip(page*10 - 10).Limit(10).ToListAsync();

        }

    }
