namespace ClothingStore.Data.Requests;

public record EditCategoryRequest(int Id, string Name) : AddCategoryRequest(Name);