using ChainStoreApi.Data;
using ChainStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Globalization;


namespace ChainStoreApi.Services;

public class OrderService
{
    private readonly IMongoCollection<Order> _OrderCollection;
    public OrderService(
        IOptions<DatabaseSetting> databaseSetting)
    {
        var mongoClient = new MongoClient(
            databaseSetting.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseSetting.Value.DatabaseName);

        _OrderCollection = mongoDatabase.GetCollection<Order>(databaseSetting.Value.OrderCollectionName);
    }

    public async Task<List<Order>> GetOrderAsync()
    {
        var data = await _OrderCollection.Find(_ => true).ToListAsync();
        return data;
    }
    public async Task<Order?> GetOrderAsync(string id) => await _OrderCollection.Find(x => x.id == id).FirstOrDefaultAsync();

    public async Task CreateOrderAsync(Order Order) => await _OrderCollection.InsertOneAsync(Order);
    public async Task UpdateOrderAsync(string id, Order Order) => await _OrderCollection.ReplaceOneAsync(x => x.id == id, Order);
    // public void UpdatePaymentMethos(string id, PaymentMethos paymentMethos) { 
    //     var idFilter = Builders<Order>.Filter.Eq(x => x.id, id);        
    //     var update = Builders<Order>.Update.Set(p => p.PaymentMethos, paymentMethos);    
    //     _OrderCollection.UpdateOne(idFilter,update);
    // }
    // public void UpdateStatus(string id, BillStatus billStatus)
    // {
    //     var idFilter = Builders<Order>.Filter.Eq(x => x.id, id);        
    //     var update = Builders<Order>.Update.Set(p => p.BillStatus, billStatus);    
    //     _OrderCollection.UpdateOne(idFilter,update);
    // }
    public async Task RemoveOrderAsync(string id) => await _OrderCollection.DeleteOneAsync(x => x.id == id);

    // public async Task<List<Order>> SearchKeyword(string? keyword)
    // {
    //     var query = await _OrderCollection.Find(_ => true).ToListAsync();
    //     if (!string.IsNullOrEmpty(keyword))
    //     {
    //         //var dateitem = DateTime.Parse(keyword).ToString();
    //         DateTime start = DateTime.ParseExact(keyword, "yyyy-MM-dd", CultureInfo.GetCultureInfo("vi-VN"));
    //         //DateTime start = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
    //         //DateTime xstart = DateTime.ParseExact(DateTime.Parse(startDate).ToString(),"yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);
    //         query = query.Where(x => x.OrderDate >= start).ToList();
    //     }
    //     return query;
    // }
    public async Task<List<Order>> SearchKeyword(string? startDate, string? endDate, string? keyword)
    {       
        var query = await _OrderCollection.Find(_ => true).ToListAsync();
        if (!string.IsNullOrEmpty(startDate))
        {
            DateTime start = DateTime.ParseExact(startDate, "yyyy-MM-dd", CultureInfo.GetCultureInfo("vi-VN"));           
            query = query.Where(x => x.OrderDate >= start).ToList();
        }

        if (!string.IsNullOrEmpty(endDate))
        {
             DateTime end = DateTime.ParseExact(endDate, "yyyy-MM-dd", CultureInfo.GetCultureInfo("vi-VN"));
            query = query.Where(x => x.LastEditDate <= end).ToList();
        }
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.CustomerName.Contains(keyword) ||
                            x.CustomerPhone.Contains(keyword)
                            ).ToList();
        }
        return query;
    }
}
