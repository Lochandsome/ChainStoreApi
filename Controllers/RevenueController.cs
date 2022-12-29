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
    [HttpGet("GetRevenueForStore")]
    public object GetRevenueForStore()
    {
        var revenue = _revenueService.GetRevenueForStore();
        return Ok(revenue);
    }
    [HttpGet("GetCalculateLastMonth")]
    public object GetCalculateLastMonth()
    {
        var revenueCLM = _revenueService.GetCalculateLastMonth();
        return Ok(revenueCLM);
    }
    [HttpGet("GetBetSellingProduct")]
    public object GetBetSellingProduct()
    {
        var revenueCLM = _revenueService.GetBetSellingProduct();
        return Ok(revenueCLM);
    }
    [HttpGet("GetCalculateLastMonthStore/{id:length(24)}")]
    public object GetCalculateLastMonthStore(string id)
    {
        var revenueCLM = _revenueService.GetCalculateLastMonthStore(id);
        return Ok(revenueCLM);
    }
    [HttpGet("GetBetSellingProductStore/{id:length(24)}")]
    public object GetBetSellingProductStore(string id)
    {
        var revenueCLM = _revenueService.GetBetSellingProductStore(id);
        return Ok(revenueCLM);
    }
}