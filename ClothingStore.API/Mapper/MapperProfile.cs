using AutoMapper;
using ClothingStore.Data.Context.Entities;
using ClothingStore.Data.Requests;

namespace ClothingStore.API.Mapper;

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
            CreateMap<EditProductRequest, Product>();
        }
    }

}