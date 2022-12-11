using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;
namespace ChainStoreApi.Models;

public class Store
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("_id")]
    public String? id {get; set;}
    public String? namestore {get; set;}=null!;
    public String? address {get; set;}=null!;

    [BsonRepresentation(BsonType.ObjectId)]
    public String? manager {get; set;}
    [BsonRepresentation(BsonType.Decimal128)]
    public List<Double> coordinates {get;set;} = null!;
    public DateTime? create_at {get;set;}= null!;

    public string? create_user {get;set;} = null!;
    public string? update_user {get;set;} = null!;

    public DateTime? update_at {get;set;}= null!;

}