

namespace ChainStoreApi.Models;
    public class AccountDTO
    {
        
        public String? username {get; set;}=null!;
        public string? password {get; set;}=null!;

         public DateTime? create_at {get;set;}= null!;
        public DateTime? update_at {get;set;}= null!;

        public string create_user {get;set;}=null!;
        public string update_user {get;set;}=null!;
        public string? role {get;set;} =null!;

    }
