using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace ChainStoreApi.Models;

public class ProfileStaff
{
    public Staff staff {get;set;} = null!;

    public Account account {get;set;} =null!;

    public ProfileStaff(Staff staff, Account account)
    {
        this.staff = staff;
        this.account = account;
    }
}