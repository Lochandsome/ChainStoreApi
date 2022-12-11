using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChainStoreApi.Models;
    public class Customer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string? id {get; set;}=null!;
        public string? first_name {get; set;}=null!;
        public string? last_name {get; set;}=null!;

        public DateTime? birthday {get;set;}=null!;
        public string? phone {get; set;}=null!;
        public string? note {get; set;}=null!;
        public string? address {get; set;} =null!;
        
        public DateTime? create_at {get;set;}= null!;
        public DateTime? update_at {get;set;}= null!;

        public string create_user {get;set;}=null!;
        public string update_user {get;set;}=null!;       


    }
