using AutoMapper;
using Day1.DTOs;
using Day1.Models;

namespace Day1.Mapper
{
    public class MapperProfile:Profile
    {

        public MapperProfile()
        {
            CreateMap<Product, ProductCreateDto>();
            CreateMap<ProductCreateDto, Product>();

            CreateMap<Product, ReadProductDto>()
            .ForMember(temp => temp.CategoryName,
                o => o.MapFrom(s => s.Category.Name))

            .ForMember(temp => temp.SupplierName,
                  o => o.MapFrom(s => s.Supplier.Name));



            CreateMap<Category, SupplierCreateDto>();
            CreateMap<Category, CategoryCreateDto>();
            CreateMap<Category, ReadCategoryDto>();
            CreateMap<Supplier, ReadSupplierDto>();
            CreateMap<SupplierCreateDto, Supplier>();
        }
    }
}
