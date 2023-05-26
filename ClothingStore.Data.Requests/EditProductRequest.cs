using ClothingStore.Data.Context.Entities;
using Microsoft.AspNetCore.Http;

namespace ClothingStore.Data.Requests;

public record EditProductRequest(string? Name, string? Description, List<int>? Categories, IFormFile? Image);