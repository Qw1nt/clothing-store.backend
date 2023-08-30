using API.Extensions;
using Application.Categories.Commands;
using Application.Categories.Queries;
using Domain.Entities;
using Domain.Common.Configurations;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Категории товаров
/// </summary>
[ApiController]
[Route("category")]
public class CategoryController: ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Конструктор класса
    /// </summary>
    public CategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Получить все категории
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(IQueryable<Category>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllCategories());
        return result.ToHttpResponse();
    }

    /// <summary>
    /// Добавить категорию
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [Authorize(Policy = IdentityConfiguration.Policy.Admin)]
    [HttpPost]
    [ProducesResponseType(typeof(List<Category>), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddCategory([FromBody] AddCategoryCommand command)
    {
        var result = await _mediator.Send(command);
        return result.ToHttpResponse();
    }

    /// <summary>
    /// Редактировать категорию
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [Authorize(Policy = IdentityConfiguration.Policy.Admin)]
    [HttpPut]
    [ProducesResponseType(typeof(Category), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EditCategory([FromBody] EditCategoryCommand command)
    {
        var result = await _mediator.Send(command);
        return result.ToHttpResponse();
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
        var result = await _mediator.Send(new DeleteCategoryCommand(categoryId));
        return result.ToHttpResponse();
    }
}