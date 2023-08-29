using Application.Common.Contracts;
using Domain.Common;
using Domain.Entities;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Application.Reviews.Commands;

public record CreateReviewForProductCommand(int UserId, CreateReviewCommand Source) : ICommand<OperationResult>;

public class CreateReviewForProductCommandHandler : ICommandHandler<CreateReviewForProductCommand, OperationResult>
{
    private readonly IApplicationDataContext _applicationDataContext;

    public CreateReviewForProductCommandHandler(IApplicationDataContext applicationDataContext)
    {
        _applicationDataContext = applicationDataContext;
    }

    public async ValueTask<OperationResult> Handle(CreateReviewForProductCommand command, CancellationToken cancellationToken)
    {
        var alreadyCreate = await _applicationDataContext.Products
            .AsNoTracking()
            .AnyAsync(x => x.Reviews != null && x.Reviews.Any(y => y.Owner.Id == command.UserId),
                cancellationToken: cancellationToken);

        if (alreadyCreate == true)
            return OperationResult.BadRequest("Вы уже оставляли отзыв на этот товар");

        var user = await _applicationDataContext.Users
            .SingleOrDefaultAsync(x => x.Id == command.UserId, cancellationToken: cancellationToken);
        
        if (user is null)
            return OperationResult.BadRequest();

        var review = new Review
        {
            Owner = user,
            Title = command.Source.Title,
            Content = command.Source.Content,
            Date = DateTime.UtcNow
        };

        var reviewEntry = await _applicationDataContext.Reviews
            .AddAsync(review, cancellationToken);

        var product = await _applicationDataContext.Products
            .SingleOrDefaultAsync(x => x.Id == command.Source.ProductId, cancellationToken: cancellationToken);
        if (product is null)
            return OperationResult.BadRequest();

        product.Reviews ??= new List<Review>();
        product.Reviews.Add(reviewEntry.Entity);

        await _applicationDataContext.SaveChangesAsync(cancellationToken);
        return OperationResult.Ok(reviewEntry.Entity);
    }
}