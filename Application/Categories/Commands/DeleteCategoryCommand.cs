using Application.Common.Contracts;
using Domain.Common;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Application.Categories.Commands;

public record DeleteCategoryCommand(int CategoryId) : ICommand<OperationResult>;

public class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand, OperationResult>
{
    private readonly IApplicationDataContext _applicationDataContext;

    public DeleteCategoryCommandHandler(IApplicationDataContext applicationDataContext)
    {
        _applicationDataContext = applicationDataContext;
    }

    public async ValueTask<OperationResult> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await _applicationDataContext.Categories
            .SingleOrDefaultAsync(x => x.Id == command.CategoryId, cancellationToken: cancellationToken);

        if (category is null)
            return OperationResult.NotFound();

        _applicationDataContext.Categories.Remove(category);
        await _applicationDataContext.SaveChangesAsync(cancellationToken);

        return OperationResult.Ok();
    }
}