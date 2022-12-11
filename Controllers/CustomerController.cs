using ChainStoreApi.Models;
using ChainStoreApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChainStoreApi.Controllers;

[ApiController]
[Route("[controller]")]
public class customerController : ControllerBase
{
    private readonly CustomerService _customerService;
    public customerController(CustomerService customerService) => _customerService = customerService;

    [HttpGet]
    public async Task<List<Customer>> Get() => await _customerService.GetCustomerAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Customer>> Get(string id)
    {
        var customer = await _customerService.GetCustomerAsync(id);
        if (customer is null)
        {
            return NotFound();
        }
        return customer;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomer(Customer customer)
    {
        await _customerService.CreateCustomerAsync(customer);

        return CreatedAtAction(nameof(Get), new { id = customer.id }, customer);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> UpdateCustomer(string id, Customer customerUpdate)
    {
        var customer = await _customerService.GetCustomerAsync(id);

        if (customer is null)
        {
            return NotFound();
        }

        customerUpdate.id = customer.id;

        await _customerService.UpdateCustomerAsync(id, customerUpdate);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> DeleteCustomer(string id)
    {
        var customer = await _customerService.GetCustomerAsync(id);

        if (customer is null)
        {
            return NotFound();
        }

        await _customerService.RemoveCustomerAsync(id);

        return NoContent();
    }

}
