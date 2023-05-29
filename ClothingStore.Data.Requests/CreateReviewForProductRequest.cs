namespace ClothingStore.Data.Requests;

public record CreateReviewForProductRequest(int ProductId, string Title, string Content);
