
using System.ComponentModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChainStoreApi.Models;

public class Order
{   
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("_id")]
    public string? id { get; set; }
    public DateTime? OrderDate { get; set; }
    public DateTime? LastEditDate { get; set; } = null!;

    
    public Staff OrderStaff { get; set; } = null!;
    public string CustomerName { get; set; } = null!;
    public string CustomerEmail { get; set; } = null!;
    public string CustomerPhone { get; set; } = null!;
    public string CustomerMessage { get; set; } = null!;
    public string CustomerAddress { get; set; } = null!;
    public BillStatus BillStatus { get; set; }
    public PaymentMethos PaymentMethos { get; set; }
    public List<OrderDetails> OrderDetails { get; set; } = null!;
    public string TotalRecord { get; set; } = null!;
}

public class OrderDetails
{
    public Product? Product { get; set; }
    public int? count { get; set; }

}

public enum PaymentMethos
{
    [Description("Cash On Delivery")]
    CashOnDelivery = 0,
    [Description("Debit Card")]
    DebitCard = 1,
    [Description("Credit Card")]
    CreditCard = 2,
    [Description("Visa")]
    Visa = 3,
    [Description("Zalo Pay")]
    ZaloPay = 4,
    [Description("Momo")]
    Momo = 5
}

public enum BillStatus
{
    [Description("New Bill")]
    NewBill = 0,
    [Description("In Progress")]
    InProgress = 1,
    [Description("Packing")]
    Packing = 2,
    [Description("Delivering")]
    Delivering = 3,
    [Description("Returned")]
    Returned = 4,
    [Description("Cancelled")]
    Cancelled = 5,
    [Description("Completed")]
    Completed = 6
}