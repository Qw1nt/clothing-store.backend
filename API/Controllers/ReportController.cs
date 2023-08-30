using API.Extensions;
using Application.Reports.Commands;
using Domain.Common.Configurations;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Формирование отчётов
/// </summary>
[ApiController]
[Route("report")]
public class ReportController : ControllerBase
{
    private readonly IMediator _mediator;
    
    /// <summary>
    /// Конструктор класса
    /// </summary>
    public ReportController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Сформировать отчёт
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [Authorize(Policy = IdentityConfiguration.Policy.Manager)]
    [HttpPost]
    public async Task<IActionResult> BuildReport([FromBody] BuildProductReportCommand command)
    {
        var result = await _mediator.Send(command);
        return result.ToHttpResponse();
    }
}