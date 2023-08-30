using Application.Common.Contracts;
using Domain.Common;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Application.Categories.Commands;

public record EditCategoryCommand(int Id, string Name) : ICommand<OperationResult>;

public class EditCategoryCommandHandler : ICommandHandler<EditCategoryCommand, OperationResult>
{
    private readonly IApplicationDataContext _applicationDataContext;

    public EditCategoryCommandHandler(IApplicationDataContext applicationDataContext)
    {
        _applicationDataContext = applicationDataContext;
    }

    public async ValueTask<OperationResult> Handle(EditCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await _applicationDataContext.Categories
            .SingleOrDefaultAsync(x => x.Id == command.Id, cancellationToken: cancellationToken);

        if (category is null)
            return OperationResult.NotFound();

        category.Name = command.Name;
        _applicationDataContext.Categories.Update(category);
        await _applicationDataContext.SaveChangesAsync(cancellationToken);

        return OperationResult.Ok(category);
    }
}