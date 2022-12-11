
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChainStoreApi.Models;

public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("_id")]
    public string? id { get; set; } = null!;
    public string? name { get; set; } = null!;
    public int? purchasePrice { get; set;}
    public int? price { get; set; }

    public string? note { get; set; } = null!;

    public List<String> detail { get; set; } = null!;

    public List<Image> image { get; set; } = null!;
    public Category category { get; set; } = null!;
    public Partner partner { get; set; } = null!;

    public DateTime? create_at { get; set; } = null!;
    public DateTime? update_at { get; set; } = null!;

    public string create_user { get; set; } = null!;
    public string update_user { get; set; } = null!;

}