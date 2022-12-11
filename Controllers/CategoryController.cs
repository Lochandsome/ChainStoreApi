using ChainStoreApi.Models;
using ChainStoreApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChainStoreApi.Controllers;

    [ApiController]
    [Route("[controller]")]
    public class categoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;
        public categoryController(CategoryService categoryService)=> _categoryService = categoryService;
        
        [HttpGet]
        public async Task<List<Category>> Get() => await _categoryService.GetCategoryAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Category>> Get(string id)
        {
            var category = await _categoryService.GetCategoryAsync(id);
            if(category is null)
            {
                return NotFound();
            }
            return category;
        }

    [HttpPost]
    public async Task<IActionResult> Post(Category category)
    {
        await _categoryService.CreateCategoryAsync(category);

        return CreatedAtAction(nameof(Get), new { id = category.id }, category);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Category categoryupdate)
    {
        var category = await _categoryService.GetCategoryAsync(id);

        if (category is null)
        {
            return NotFound();
        }

        category.id = categoryupdate.id;

        await _categoryService.UpdateCategoryAsync(id, categoryupdate);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var category = await _categoryService.GetCategoryAsync(id);

        if (category is null)
        {
            return NotFound();
        }

        await _categoryService.RemoveCategoryAsync(id);

        return NoContent();
    }

    }
