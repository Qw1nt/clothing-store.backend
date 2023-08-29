using Domain.Entities;

namespace Application.Common.Contracts;

public interface IUpdateProductService
{
    Task SetProductCategories(List<int> ids, Product product);
}