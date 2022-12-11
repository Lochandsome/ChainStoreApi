using ChainStoreApi.EnumExtension;
using ChainStoreApi.Models;
using ChainStoreApi.Services;
using Microsoft.AspNetCore.Mvc;
using static ChainStoreApi.Services.RevenueService;

namespace ChainStoreApi.Controllers;

[ApiController]
[Route("[controller]")]
public class revenueController : ControllerBase
{
    private readonly RevenueService _revenueService;
    public revenueController(RevenueService revenueService) => _revenueService = revenueService;

    [HttpGet("RevenueByYear")]
    public async Task<List<revenue>> GetRevenueByYears() => await _revenueService.GetRevenueByYears();
    [HttpGet("RevenueByWeek")]
    public async Task<ActionResult<revenue>> GetRevenueByWeek(string? startDate, string? endDate)
    {
        var revenue = await _revenueService.GetRevenueByWeek(startDate, endDate);
        return Ok(revenue);
    }
}