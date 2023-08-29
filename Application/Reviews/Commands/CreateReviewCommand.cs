namespace Application.Reviews.Commands;

public record CreateReviewCommand(int ProductId, string Title, string Content);
