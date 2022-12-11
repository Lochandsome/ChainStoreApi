
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace ChainStoreApi.Models;
    public class TimeWork
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string? id {get;set;} = null!;

        [BsonRepresentation(BsonType.ObjectId)]
        public string? idstore{get;set;}= null!;
        
        public List<TimeShift> timeshift {get;set;} =null!;

        public DateTime? create_at {get;set;}= null!;
        public DateTime? update_at {get;set;}= null!;

        public string create_user {get;set;}=null!;
        public string update_user {get;set;}=null!;
        

    }
