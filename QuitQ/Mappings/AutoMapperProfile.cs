using AutoMapper;
using QuitQ.DTOs.AddressDTOs;
using QuitQ.DTOs.CategoryDTOs;
using QuitQ.DTOs.OrderDTOs;
using QuitQ.DTOs.PaymentDTOs;
using QuitQ.DTOs.ProductDTOs;
using QuitQ.DTOs.SellerDTOs;
using QuitQ.DTOs.SubCategoryDTO;
using QuitQ.DTOs.UserDTOs;
using QuitQ.Models;

namespace QuitQ.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Product -> ResponseDTO
            CreateMap<Product, ProductResponseDTO>()
                .ForMember(dest => dest.QuantityAvailable,
                    opt => opt.MapFrom(src =>
                        src.Inventory != null
                            ? src.Inventory.QuantityAvailable
                            : 0))
                .ForMember(dest => dest.SubCategoryName,
                    opt => opt.MapFrom(src =>
                        src.SubCategory!.SubCategoryName))
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src =>
                        src.SubCategory!.Category!.CategoryName))
                .ForMember(dest => dest.SellerName,
                    opt => opt.MapFrom(src =>
                        src.Seller!.StoreName));

            // CreateDTO -> Product
            CreateMap<ProductCreateDTO, Product>()
                .ForMember(dest => dest.ProductId,
                    opt => opt.Ignore())
                .ForMember(dest => dest.SellerId,
                    opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt,
                    opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt,
                    opt => opt.Ignore())
                .ForMember(dest => dest.IsActive,
                    opt => opt.Ignore());

            // UpdateDTO -> Product
            CreateMap<ProductUpdateDTO, Product>()
                .ForMember(dest => dest.ProductId,
                    opt => opt.Ignore())
                .ForMember(dest => dest.SellerId,
                    opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt,
                    opt => opt.Ignore());

            CreateMap<Category, CategoryResponseDTO>();

            CreateMap<CategoryCreateDTO, Category>()
                .ForMember(dest => dest.CategoryId,
                    opt => opt.Ignore());

            CreateMap<CategoryUpdateDTO, Category>()
                .ForMember(dest => dest.CategoryId,
                    opt => opt.Ignore());

            CreateMap<SubCategory, SubCategoryResponseDTO>()
    .ForMember(dest => dest.CategoryName,
        opt => opt.MapFrom(src =>
            src.Category!.CategoryName));

            CreateMap<SubCategoryCreateDTO, SubCategory>()
                .ForMember(dest => dest.SubCategoryId,
                    opt => opt.Ignore());

            CreateMap<SubCategoryUpdateDTO, SubCategory>()
                .ForMember(dest => dest.SubCategoryId,
                    opt => opt.Ignore());

            CreateMap<Seller, SellerResponseDTO>()
    .ForMember(dest => dest.UserName,
        opt => opt.MapFrom(src =>
            src.User!.Name));

            CreateMap<SellerCreateDTO, Seller>()
                .ForMember(dest => dest.SellerId,
                    opt => opt.Ignore())
                .ForMember(dest => dest.UserId,
                    opt => opt.Ignore());

            CreateMap<SellerUpdateDTO, Seller>()
                .ForMember(dest => dest.SellerId,
                    opt => opt.Ignore())
                .ForMember(dest => dest.UserId,
                    opt => opt.Ignore());

            CreateMap<Address, AddressResponseDTO>()
    .ForMember(dest => dest.UserName,
        opt => opt.MapFrom(src =>
            src.User!.Name));

            CreateMap<AddressCreateDTO, Address>()
                .ForMember(dest => dest.AddressId,
                    opt => opt.Ignore())
                .ForMember(dest => dest.UserId,
                    opt => opt.Ignore());

            CreateMap<AddressUpdateDTO, Address>()
                .ForMember(dest => dest.AddressId,
                    opt => opt.Ignore())
                .ForMember(dest => dest.UserId,
                    opt => opt.Ignore());

            CreateMap<Payment, PaymentResponseDTO>();

            CreateMap<OrderItem, OrderItemResponseDTO>()
    .ForMember(dest => dest.ProductName,
        opt => opt.MapFrom(src =>
            src.Product!.ProductName))
    .ForMember(dest => dest.SubTotal,
        opt => opt.MapFrom(src =>
            src.Quantity * src.PriceAtPurchase));

            CreateMap<Order, OrderResponseDTO>()
                .ForMember(dest => dest.CustomerName,
                    opt => opt.MapFrom(src =>
                        src.User!.Name))
                .ForMember(dest => dest.FullAddress,
                    opt => opt.MapFrom(src =>
                        src.Address!.FullAddress))
                .ForMember(dest => dest.City,
                    opt => opt.MapFrom(src =>
                        src.Address!.City))
                .ForMember(dest => dest.State,
                    opt => opt.MapFrom(src =>
                        src.Address!.State))
                .ForMember(dest => dest.Pincode,
                    opt => opt.MapFrom(src =>
                        src.Address!.Pincode))
                .ForMember(dest => dest.Country,
                    opt => opt.MapFrom(src =>
                        src.Address!.Country))
                .ForMember(dest => dest.OrderItems,
                    opt => opt.MapFrom(src =>
                        src.OrderItems));

            CreateMap<OrderItem, SellerOrderResponseDTO>()
                .ForMember(dest => dest.CustomerName,
                    opt => opt.MapFrom(src =>
                        src.Order!.User!.Name))
                .ForMember(dest => dest.ProductName,
                    opt => opt.MapFrom(src =>
                        src.Product!.ProductName))
                .ForMember(dest => dest.OrderStatus,
                    opt => opt.MapFrom(src =>
                        src.Order!.OrderStatus))
                .ForMember(dest => dest.OrderDate,
                    opt => opt.MapFrom(src =>
                        src.Order!.OrderDate));


            CreateMap<User, UserResponseDTO>()
    .ForMember(dest => dest.RoleName,
        opt => opt.MapFrom(src =>
            src.Role!.RoleName));

            CreateMap<UserUpdateDTO, User>()
                .ForMember(dest => dest.UserId,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Email,
                    opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash,
                    opt => opt.Ignore())
                .ForMember(dest => dest.RoleId,
                    opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt,
                    opt => opt.Ignore())
                .ForMember(dest => dest.IsActive,
                    opt => opt.Ignore());
        }
    }
}
