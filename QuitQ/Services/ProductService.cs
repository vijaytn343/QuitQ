using Microsoft.EntityFrameworkCore;
using QuitQ.Data;
using QuitQ.DTOs.ProductDTOs;
using QuitQ.Models;
using QuitQ.Services.Interfaces;
namespace QuitQ.Services
{
    public class ProductService:IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ProductResponseDTO>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Seller)
                .Include(p => p.SubCategory)
                    .ThenInclude(sc => sc.Category)
                .Include(p => p.Inventory)
                .Select(p => new ProductResponseDTO
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Price = p.Price,
                    Brand = p.Brand,
                    ImageUrl = p.ImageUrl,
                    IsActive = p.IsActive,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    QuantityAvailable = p.Inventory != null ? p.Inventory.QuantityAvailable : 0,
                    CategoryName = p.SubCategory!.Category!.CategoryName,
                    SubCategoryName = p.SubCategory.SubCategoryName,
                    SellerName = p.Seller!.StoreName
                })
                .ToListAsync();
        }
        public async Task<ProductResponseDTO?> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Seller)
                .Include(p => p.SubCategory)
                    .ThenInclude(sc => sc.Category)
                .Include(p => p.Inventory)
                .Where(p => p.ProductId == id)
                .Select(p => new ProductResponseDTO
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Price = p.Price,
                    Brand = p.Brand,
                    ImageUrl = p.ImageUrl,
                    IsActive = p.IsActive,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    QuantityAvailable = p.Inventory != null ? p.Inventory.QuantityAvailable : 0,
                    CategoryName = p.SubCategory!.Category!.CategoryName,
                    SubCategoryName = p.SubCategory.SubCategoryName,
                    SellerName = p.Seller!.StoreName
                })
                .FirstOrDefaultAsync();
        }
        public async Task<ProductResponseDTO> CreateProductAsync(int userId,ProductCreateDTO dto)
        {
            var seller = await _context.Sellers.FirstOrDefaultAsync(s => s.UserId == userId);

            if (seller == null)
                throw new Exception("Seller profile not found.");

            var product = new Product
            {
                SellerId = seller.SellerId,
                SubCategoryId = dto.SubCategoryId,
                ProductName = dto.ProductName,
                Description = dto.Description,
                Price = dto.Price,
                Brand = dto.Brand,
                ImageUrl = dto.ImageUrl,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = true
            };

            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            var inventory = new Inventory
            {
                ProductId = product.ProductId,
                QuantityAvailable = dto.QuantityAvailable,
                LastUpdated = DateTime.Now
            };

            _context.Inventories.Add(inventory);

            await _context.SaveChangesAsync();


            var savedProduct = await _context.Products
    .Include(p => p.Seller)
    .Include(p => p.SubCategory)
        .ThenInclude(sc => sc.Category)
    .Include(p => p.Inventory)
    .FirstOrDefaultAsync(p => p.ProductId == product.ProductId);

            return new ProductResponseDTO
            {
                ProductId = savedProduct!.ProductId,
                ProductName = savedProduct.ProductName,
                Description = savedProduct.Description,
                Price = savedProduct.Price,
                Brand = savedProduct.Brand,
                ImageUrl = savedProduct.ImageUrl,
                IsActive = savedProduct.IsActive,
                CreatedAt = savedProduct.CreatedAt,
                UpdatedAt = savedProduct.UpdatedAt,

                QuantityAvailable = savedProduct.Inventory!.QuantityAvailable,

                CategoryName = savedProduct.SubCategory!.Category!.CategoryName,
                SubCategoryName = savedProduct.SubCategory.SubCategoryName,
                SellerName = savedProduct.Seller!.StoreName
            };
        }
        public async Task<bool> UpdateProductAsync(int userId,int productId, ProductUpdateDTO dto)
        {
            var seller = await _context.Sellers.FirstOrDefaultAsync(s => s.UserId == userId);

            if (seller == null)
                return false;

            var product = await _context.Products
                .FirstOrDefaultAsync(p =>
                    p.ProductId == productId &&
                    p.SellerId == seller.SellerId);

            if (product == null)
                return false;

            if (product == null)
                return false;

            product.SubCategoryId = dto.SubCategoryId;
            product.ProductName = dto.ProductName;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.Brand = dto.Brand;
            product.ImageUrl = dto.ImageUrl;
            product.IsActive = dto.IsActive;
            product.UpdatedAt = DateTime.Now;

            var inventory = await _context.Inventories
                .FirstOrDefaultAsync(i => i.ProductId == product.ProductId);

            if (inventory != null)
            {
                inventory.QuantityAvailable = dto.QuantityAvailable;
                inventory.LastUpdated = DateTime.Now;
            }

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteProductAsync(int userId,int productId)
        {
            var seller = await _context.Sellers.FirstOrDefaultAsync(s => s.UserId == userId);

            if (seller == null)
                return false;

            var product = await _context.Products.FirstOrDefaultAsync(p =>p.ProductId == productId &&p.SellerId == seller.SellerId);

            if (product == null)
                return false;

            _context.Products.Remove(product);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
