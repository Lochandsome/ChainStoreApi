using ChainStoreApi.Models;


namespace ChainStoreApi.Services;

public class ProfileStaffService
{
    private readonly StaffService _staffService;
    private readonly AccountService _accountService;

    public ProfileStaffService(StaffService staffService, AccountService accountService)
    {
        
        
    
        _staffService = staffService;
        _accountService = accountService;
    }


    // Get Collection Staff and Account 
    // Convert 2 collection to ProfileStaff
    public async Task<ProfileStaff?> GetProfileStaffAsync(string idStaff)
    {
        var staff = await _staffService.GetStaffAsync(idStaff);

        var account = await _accountService.GetAccountAsync(staff!.accountId);
        
        return new ProfileStaff(staff,account!);
    }

    // public async Task<List<ProfileStaff?> GetProfileStaffAsync()
    // {

    // }
    public async Task<List<ProfileStaff>> GetProfileManagerAsync()
    {
        // get account with role manager
        var account = await _accountService.GetAccountManagerAsync();
        // get staff with account is mangager
        List<ProfileStaff> result = new List<ProfileStaff>();
        foreach(var item in account)
        {
            var staff = await _staffService.GetStaffWithAccountIdAsync(item.id!);
            ProfileStaff prs = new ProfileStaff(staff!,item);
            result.Add(prs);
        }
        return result;
    }
    
}
