using ChainStoreApi.Models;
using ChainStoreApi.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
namespace ChainStoreApi.Controllers;
using System.Collections;
using Microsoft.AspNetCore.Authorization;

    [ApiController]
    [Route("[controller]")]
    public class storeController : ControllerBase
    {
        private readonly StoreService _storeService;
        private readonly StaffService _staffService;
        // public storeController(StoreService storeService)=> _storeService = storeService;
        public storeController(StoreService storeService, StaffService staffService)
        {
            _staffService = staffService;
            _storeService = storeService;
        }
        
        // [HttpGet]
        // public async Task<List<Store>> Get() => await _storeService.GetStoreAsync();
        [Authorize(Roles ="admin , staff")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = new ArrayList();
            var store = await _storeService.GetStoreAsync();
            foreach(var st in store)
            {
                var manager = await _staffService.GetStaffAsync(st.manager!);
                var sumStaff = await _staffService.GetStaffInStoreAsync(st.id!);
                var storeStaff = new ArrayList();
                storeStaff.Add(st);
                storeStaff.Add(manager);
                storeStaff.Add(sumStaff.Count());
                storeStaff.ToJson();
                result.Add(storeStaff);
                
            }
            return new JsonResult(result);
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Store>> Get(string id)
        {
            var account = await _storeService.GetStoreAsync(id);
            if(account is null)
            {
                return NotFound();
            }
            return account;
        }


    [HttpPost]
    public async Task<IActionResult> Post(Store account)
    {
        await _storeService.CreateStoreAsync(account);

        return CreatedAtAction(nameof(Get), new { id = account.id }, account);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Store accountupdate)
    {
        var account = await _storeService.GetStoreAsync(id);

        if (account is null)
        {
            return NotFound();
        }

        account.id = accountupdate.id;

        await _storeService.UpdateStoreAsync(id, accountupdate);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var staff = await _storeService.GetStoreAsync(id);

        if (staff is null)
        {
            return NotFound();
        }

        await _storeService.RemoveStoreAsync(id);

        return NoContent();
    }

    }
