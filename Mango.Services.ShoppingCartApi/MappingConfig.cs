using AutoMapper;
using Mango.Services.ShoppingCartApi.Models;
using Mango.Services.ShoppingCartApi.Models.DTO;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Mango.Services.ShoppingCartApi
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps ()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CartHeader, CartHeaderdto>();
                config.CreateMap<CartHeaderdto, CartHeader>();
                config.CreateMap<CartDetails, CartDetailDto>();
                config.CreateMap<CartDetailDto, CartDetails>();
            });
            return mappingConfig;
            
        }
    }
}
