using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChainStoreApi.Models;

public class RefreshToken
{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string? id {get; set;}=null!;
        
        public string? refeshToken {get;set;} = null!;
        public Account? account {get;set;}

        public RefreshToken(string refeshToken, Account account)
        {
            this.account = account;
            this.refeshToken = refeshToken;
        }

}