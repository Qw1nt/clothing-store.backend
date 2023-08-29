namespace Application;

public record EditCategoryRequest(int Id, string Name) : AddCategoryRequest(Name);