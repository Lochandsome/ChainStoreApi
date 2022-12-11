using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChainStoreApi.Models;
    public class Account
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public String? id {get; set;}
        public String? username {get; set;}=null!;
        public byte[]? password {get; set;}=null!;

        public byte[]? salt {get;set;} = null!;

        public string? role {get;set;} = null!;
        
        public DateTime? create_at {get;set;}= null!;
        public DateTime? update_at {get;set;}= null!;

        public string create_user {get;set;}=null!;
        public string update_user {get;set;}=null!;


    }
