using ChainStoreApi.EnumExtension;
using ChainStoreApi.Models;
using ChainStoreApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChainStoreApi.Controllers;

[ApiController]
[Route("[controller]")]
public class orderController : ControllerBase
{
    private readonly OrderService _orderService;
    public orderController(OrderService orderService) => _orderService = orderService;

    [HttpGet("GetBillStatus")]
    public IActionResult GetBillStatus()
    {
        List<EnumValue> enums = ((BillStatus[])Enum.GetValues(typeof(BillStatus)))
        .Select(x => new EnumValue()
        {
            Value = (int)x,
            Name = x.GetDescription()
        }).ToList();
        return new OkObjectResult(enums);
    }
    // [HttpPut("UpdateStatus/{id:length(24)}")]
    // public IActionResult UpdateStatus(string id, BillStatus billStatus)
    // {
    //     var status = _orderService.GetOrderAsync(id);
    //     if (status is null)
    //     {
    //         return NotFound();
    //     }
    //     _orderService.UpdateStatus(id, billStatus);
    //     return new OkResult();
    // }
    [HttpGet("GetPaymentMethos")]
    public IActionResult GetPaymentMethos()
    {
        List<EnumValue> enums = ((PaymentMethos[])Enum.GetValues(typeof(PaymentMethos)))
        .Select(x => new EnumValue()
        {
            Value = (int)x,
            Name = x.GetDescription()
        }).ToList();
        return new OkObjectResult(enums);
    }
    // [HttpPut("UpdatePaymentMethos/{id:length(24)}")]
    // public IActionResult UpdatePaymentMethos(string id, PaymentMethos paymentMethos)
    // {
    //     var status = _orderService.GetOrderAsync(id);
    //     if (status is null)
    //     {
    //         return NotFound();
    //     }
    //     _orderService.UpdatePaymentMethos(id, paymentMethos);
    //     return new OkResult();
    // }
    [HttpGet("Keyword")]
    public async Task<ActionResult<Order>> searchkeyword(string? startDate, string? endDate, string? keyword)
    {
        var order = await _orderService.SearchKeyword(startDate, endDate, keyword);
        return Ok(order);
    }
    // [HttpGet("Keyword")]
    // public async Task<ActionResult<Order>> searchkeyword(string? keyword)
    // {
    //     var order = await _orderService.SearchKeyword(keyword);
    //     return Ok(order);
    // }

    [HttpGet]
    public async Task<List<Order>> Get() => await _orderService.GetOrderAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Order>> Get(string id)
    {
        var order = await _orderService.GetOrderAsync(id);
        if (order is null)
        {
            return NotFound();
        }
        return order;
    }

    [HttpPost]
    public async Task<IActionResult> Createorder(Order order)
    {
        await _orderService.CreateOrderAsync(order);

        return CreatedAtAction(nameof(Get), new { id = order.id }, order);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> UpdateOrder(string id, Order orderUpdate)
    {
        var order = await _orderService.GetOrderAsync(id);

        if (order is null)
        {
            return NotFound();
        }

        orderUpdate.id = order.id;

        await _orderService.UpdateOrderAsync(id, orderUpdate);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> DeleteOrder(string id)
    {
        var order = await _orderService.GetOrderAsync(id);

        if (order is null)
        {
            return NotFound();
        }

        await _orderService.RemoveOrderAsync(id);

        return NoContent();
    }

}
