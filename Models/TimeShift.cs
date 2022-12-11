using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace ChainStoreApi.Models;
    public class TimeShift
    {
        public String? name {get; set;}=null!;
        public TimeSpan? timeStart {get; set;}=null!;
        public TimeSpan? timeEnd {get; set;}=null!;
    }