using Application;
using Application.Products.Commands;
using AutoMapper;
using Domain.Entities;

namespace API.Mapper;

/// <summary>
/// 
/// </summary>
public class MapperProfile
{
    /// <summary>
    /// Профиль для товаров и услуг
    /// </summary>
    public class ProductProfile : Profile
    {
        /// <summary>
        /// Конструктор клааса. В нём объявляются все карты для маппинга
        /// </summary>
        public ProductProfile()
        {
            CreateMap<EditProductCommand, Product>();
        }
    }

}