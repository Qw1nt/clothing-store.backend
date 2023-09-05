using Application.Products.Commands;
using Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Application.Common.Mapper;

[Mapper]
public static partial class ProductMapper
{
    public static partial Product ToProduct(this AddProductCommand command);
}