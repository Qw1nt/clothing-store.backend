using Application.Categories.Commands;
using Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Application.Common.Mapper;

[Mapper]
public static partial class CategoryMapper
{
    public static partial Category ToCategory(this AddCategoryCommand command);
}