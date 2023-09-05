using Application.Common.Contracts;
using Application.Common.Mapper;
using Domain.Common;
using Domain.Entities;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Application.Categories.Commands;

public record AddCategoryCommand(string Name) : ICommand<OperationResult>;

public class AddCategoryCommandHandler : ICommandHandler<AddCategoryCommand, OperationResult>
{
    private readonly IApplicationDataContext _applicationDataContext;

    public AddCategoryCommandHandler(IApplicationDataContext applicationDataContext)
    {
        _applicationDataContext = applicationDataContext;
    }

    public async ValueTask<OperationResult> Handle(AddCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = command.ToCategory();

        await _applicationDataContext.Categories.AddAsync(category, cancellationToken);
        await _applicationDataContext.SaveChangesAsync(cancellationToken);

        return OperationResult.Ok(_applicationDataContext.Categories.AsNoTracking());
    }
}