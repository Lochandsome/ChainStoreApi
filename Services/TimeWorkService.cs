using ChainStoreApi.Data;
using ChainStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;


namespace ChainStoreApi.Services;

    public class TimeWorkService
    {
        private readonly IMongoCollection<TimeWork> _timeWorkCollection;
        public TimeWorkService(
            IOptions<DatabaseSetting> databaseSetting)
        {
            var mongoClient = new MongoClient(
                databaseSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(databaseSetting.Value.DatabaseName);

            _timeWorkCollection = mongoDatabase.GetCollection<TimeWork>(databaseSetting.Value.TimeWorkCollectionName);
        }

        public async Task<TimeWork?> GetTimeWorkWithStoreAsync(string idstore)=> await _timeWorkCollection.Find(x => x.idstore == idstore).FirstOrDefaultAsync();

        public async Task<TimeWork?> GetTimeWorkAsync(string id) => await _timeWorkCollection.Find(x => x.id == id).FirstOrDefaultAsync();
        public async Task CreateTimeWorkAsync(TimeWork timeWork) => await _timeWorkCollection.InsertOneAsync(timeWork);
        public async Task UpdateTimeWorkAsync(string id, TimeWork timeWork) => await _timeWorkCollection.ReplaceOneAsync(x => x.id == id, timeWork);
        public async Task RemoveTimeWorkAsync(string id) => await _timeWorkCollection.DeleteOneAsync(x=> x.id == id);


       
    }
