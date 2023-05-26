using ClothingStore.Configurations;
using ClothingStore.Data.Context;
using ClothingStore.Data.Context.Entities;
using ClothingStore.Data.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.API.Controllers;

/// <summary>
/// Категории товаров
/// </summary>
[ApiController]
[Route("category")]
public class CategoryController: ControllerBase
{
    private readonly DataContext _dataContext;

    /// <summary>
    /// Конструктор класса
    /// </summary>
    /// <param name="dataContext"></param>
    public CategoryController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    /// <summary>
    /// Получить все категории
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(IQueryable<Category>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        var result = _dataContext.Categories
            .AsNoTracking();

        return Ok(result);
    }

    /// <summary>
    /// Добавить категорию
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [Authorize(Policy = IdentityConfiguration.Policy.Admin)]
    [HttpPost]
    [ProducesResponseType(typeof(List<Category>), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddCategory([FromBody] AddCategoryRequest request)
    {
        var category = new Category()
        {
            Name = request.Name
        };

        await _dataContext.Categories.AddAsync(category);
        await _dataContext.SaveChangesAsync();

        return Ok(_dataContext.Categories.AsNoTracking());
    }

    /// <summary>
    /// Редактировать категорию
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [Authorize(Policy = IdentityConfiguration.Policy.Admin)]
    [HttpPut]
    [ProducesResponseType(typeof(Category), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EditCategory([FromBody] EditCategoryRequest request)
    {
        var category = await _dataContext.Categories
            .FirstOrDefaultAsync(x => x.Id == request.Id);

        if (category is null)
            return NotFound();

        category.Name = request.Name;
        _dataContext.Categories.Update(category);
        await _dataContext.SaveChangesAsync();

        return Ok(category);
    }

    /// <summary>
    /// Удалить категорию
    /// </summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    [Authorize(Policy = IdentityConfiguration.Policy.Admin)]
    [HttpDelete("{categoryId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCategory([FromRoute] int categoryId)
    {
        var category = await _dataContext.Categories
            .FirstOrDefaultAsync(x => x.Id == categoryId);

        if (category is null)
            return NotFound();

        _dataContext.Categories.Remove(category);
        await _dataContext.SaveChangesAsync();

        return Ok();
    }
}