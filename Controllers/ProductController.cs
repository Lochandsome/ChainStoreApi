using ChainStoreApi.Models;
using ChainStoreApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChainStoreApi.Controllers;

    [ApiController]
    [Route("[controller]")]
    public class productController : ControllerBase
    {
        private readonly ProductService _productService;
        public productController(ProductService productService)=> _productService = productService;
        
        [HttpGet]
        public async Task<List<Product>> Get() => await _productService.GetProductAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Product>> Get(string id)
        {
            var product = await _productService.GetProductAsync(id);
            if(product is null)
            {
                return NotFound();
            }
            return product;
        }

        [HttpGet("withcategory/{idcategory:length(24)}")]
        public async Task<Object> GetWithCategory(string idcategory, [FromQuery] int? page)
        {
            var product = await _productService.GetProductsAsyncWithCategory(idcategory, page);
            
            if(product is null)
                return BadRequest("Product is null");
            return product;
        }
    [HttpPost]
    public async Task<IActionResult> Post(Product product)
    {
        await _productService.CreateProductAsync(product);

        return CreatedAtAction(nameof(Get), new { id = product.id }, product);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Product productUpdate)
    {
        var product = await _productService.GetProductAsync(id);

        if (product is null)
        {
            return NotFound();
        }

        product.id = productUpdate.id;

        await _productService.UpdateProductAsync(id, productUpdate);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var product = await _productService.GetProductAsync(id);

        if (product is null)
        {
            return NotFound();
        }

        await _productService.RemoveProductAsync(id);

        return NoContent();
    }

    }
