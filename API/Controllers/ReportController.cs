using System.Text.Json;
using Application;
using Domain;
using Domain.Entities;
using Domain.Common;
using Domain.Common.Configurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

/// <summary>
/// Формирование отчётов
/// </summary>
[ApiController]
[Route("report")]
public class ReportController : ControllerBase
{
    private readonly DataContext _dataContext;

    /// <summary>
    /// Конструктор класса
    /// </summary>
    /// <param name="dataContext"></param>
    public ReportController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    /// <summary>
    /// Сформировать отчёт
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [Authorize(Policy = IdentityConfiguration.Policy.Manager)]
    [HttpPost]
    public async Task<IActionResult> BuildReport([FromBody] BuildProductReportRequest request)
    {
        var products = await _dataContext.Products
            .AsNoTracking()
            .Where(x => request.ProductsIds.Contains(x.Id))
            .ToListAsync();

        var orders = await _dataContext.Orders
            .AsNoTracking()
            .Where(x => x.Date > request.StartDate && x.Date < request.EndDate)
            .ToListAsync();

        var result = new List<ReportRecord>();
        
        foreach (var product in products)
        {
            var record = new ReportRecord {Product = product};

            foreach (var order in orders)
            {
                var data = JsonSerializer.Deserialize<List<CartItem>>(order.JsonData);

                var cartItem = data?.FirstOrDefault(x => x.Product.Id == product.Id);
                
                if (cartItem != null)
                    record.Count += cartItem.Count;
            }

            result.Add(record);
        }

        return Ok(result);
    }
}