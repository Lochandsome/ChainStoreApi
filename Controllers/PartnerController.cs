using ChainStoreApi.Models;
using ChainStoreApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChainStoreApi.Controllers;

    [ApiController]
    [Route("[controller]")]
    public class partnerController : ControllerBase
    {
        private readonly PartnerService _partnerService;
        public partnerController(PartnerService partnerService)=> _partnerService = partnerService;
        
        [HttpGet]
        public async Task<List<Partner>> Get() => await _partnerService.GetPartnerAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Partner>> Get(string id)
        {
            var partner = await _partnerService.GetPartnerAsync(id);
            if(partner is null)
            {
                return NotFound();
            }
            return partner;
        }

    [HttpPost]
    public async Task<IActionResult> Post(Partner partner)
    {
        await _partnerService.CreatePartnerAsync(partner);

        return CreatedAtAction(nameof(Get), new { id = partner.id }, partner);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Partner partnerupdate)
    {
        var partner = await _partnerService.GetPartnerAsync(id);

        if (partner is null)
        {
            return NotFound();
        }

        partner.id = partnerupdate.id;

        await _partnerService.UpdatePartnerAsync(id, partnerupdate);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var category = await _partnerService.GetPartnerAsync(id);

        if (category is null)
        {
            return NotFound();
        }

        await _partnerService.RemovePartnerAsync(id);

        return NoContent();
    }

    }
