using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChainStoreApi.Models;

public class Partner
{
    [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string? id {get; set;}=null!;
        public string? name {get; set;}=null!;
        public string? address {get; set;}=null!;

        public String? logo {get;set;} =null!;
        public string? note {get; set;}=null!;

        public DateTime? create_at {get;set;}= null!;
        public DateTime? update_at {get;set;}= null!;

        public string create_user {get;set;}=null!;
        public string update_user {get;set;}=null!;

}