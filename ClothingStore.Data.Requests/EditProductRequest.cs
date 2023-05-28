using ClothingStore.Data.Context.Entities;
using Microsoft.AspNetCore.Http;

namespace ClothingStore.Data.Requests;

public record EditProductRequest(string? Name, string? Description, double? Price, string? Color, List<int>? Categories, IFormFile? Image);