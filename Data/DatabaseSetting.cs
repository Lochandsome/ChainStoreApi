namespace ChainStoreApi.Data;

    public class DatabaseSetting
    {
        public string ConnectionString {get;set;} = null!;
        public string DatabaseName {get;set;} = null!;
        public string StaffCollectionName {get;set;} = null!;
        public string ProductCollectionName {get;set;} = null!;
        public string AccountCollectionName {get;set;} = null!;
        public string StoreCollectionName {get;set;} = null!;
        public string CategoryCollectionName {get;set;} = null!;
        public string PartnerCollectionName {get;set;} = null!;
        public string TimeWorkCollectionName {get;set;} = null!;
        public string RefreshTokenCollectionName {get;set;} = null!;
        public string CustomerCollectionName {get;set;} = null!;
        public string OrderCollectionName {get;set;} = null!;

    }
