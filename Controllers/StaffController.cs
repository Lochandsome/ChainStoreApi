using ChainStoreApi.Models;
using ChainStoreApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace ChainStoreApi.Controllers;

[ApiController]
[Route("[controller]")]
public class StaffController : ControllerBase
{
    private readonly StaffService _staffService;

    private readonly ProfileStaffService _profileStaffServie;

    //  public StaffController(StaffService stafftService)=> _staffService = stafftService;
    public StaffController(StaffService staffService, ProfileStaffService profileStaffServie)
    {
        _staffService = staffService;
        _profileStaffServie = profileStaffServie;

    }

    [HttpGet()]
    public async Task<List<Staff>> GetStaffOfStore(string idstore) => await _staffService.GetStaffAsyncStore(idstore);

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<ProfileStaff>> Get(string id)
    {


        var profile = await _profileStaffServie.GetProfileStaffAsync(id);
        if (profile is null)
            return NotFound();

        return profile;
    }
    [Authorize(Roles = "admin")]
    [HttpGet("manager")]
    public async Task<ActionResult<List<ProfileStaff>>> GetStaff()
    {
        var profileStaff = await _profileStaffServie.GetProfileManagerAsync();
        if (profileStaff is null)
            return BadRequest("Không có manager nào!");
        return profileStaff;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Staff staff)
    {
        await _staffService.CreateStaffAsync(staff);

        return CreatedAtAction(nameof(Get), new { id = staff.id }, staff);
    }


    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Staff staffupdate)
    {
        var staff = await _staffService.GetStaffAsync(id);

        if (staff is null)
        {
            return NotFound();
        }

        staff.id = staffupdate.id;

        await _staffService.UpdateStaffAsync(id, staffupdate);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var staff = await _staffService.GetStaffAsync(id);

        if (staff is null)
        {
            return NotFound();
        }

        await _staffService.RemoveStaffAsync(id!);

        return NoContent();
    }
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery(Name = "staff")] string? staff, [FromQuery(Name = "store")] string? store, [FromQuery(Name = "page")] int? page)
    {
        return Ok(await _staffService.GetSearchStaff(staff, store, page));
    }
    [HttpGet("withaccountid/{id:length(24)}")]
    public async Task<ActionResult<Staff>> getStaffWithAccountId(string id)
    {
        var staff = await _staffService!.GetStaffWithAccountIdAsync(id);
        if (staff is null)
            return NotFound();
        return staff;
    }
    [HttpGet("AllStaff")]
    public async Task<List<Staff>> GetAllStaff()
    {
        var staff = await _staffService.GetAllStaff();
        return staff;
    }
}
