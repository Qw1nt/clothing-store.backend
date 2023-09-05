using Application.Products.Commands;
using Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Application.Common.Mappers;

[Mapper]
public static partial class ProductMapper
{
    public static partial Product FromAddCommand(AddProductCommand command);

    public static partial Product FromEditCommand(EditProductCommand command);
}