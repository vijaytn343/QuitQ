using Microsoft.EntityFrameworkCore;
using QuitQ.Data;
using QuitQ.DTOs.ProductDTOs;
using QuitQ.Models;
using AutoMapper;
using Microsoft.Extensions.Logging;
using QuitQ.DTOs.Common;
namespace QuitQ.Services.ProductFeature
{
    public class ProductService:IProductService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;

        public ProductService(
      AppDbContext context,
      IMapper mapper,
      ILogger<ProductService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<IEnumerable<ProductResponseDTO>> GetAllProductsAsync()
        {

            var products = await _context.Products
                .Include(p => p.Seller)
                .Include(p => p.SubCategory)
                    .ThenInclude(sc => sc.Category)
                .Include(p => p.Inventory).Where(p => p.IsActive)
                .ToListAsync();

            return _mapper.Map<List<ProductResponseDTO>>(products);
        }
        public async Task<PagedResultDTO<ProductResponseDTO>>
  GetProductsAsync(ProductFilterDTO filter)
        {
            var query = _context.Products
                .Include(p => p.Seller)
                .Include(p => p.SubCategory)
                    .ThenInclude(sc => sc.Category)
                .Include(p => p.Inventory)
                .AsQueryable();
            query = query.Where(p => p.IsActive);

            if (filter.MinPrice > filter.MaxPrice)
            {
                throw new BadHttpRequestException(
     "MinPrice cannot be greater than MaxPrice");
            }

            // Search
            if (!string.IsNullOrWhiteSpace(filter.Keyword))
            {
                var keyword = filter.Keyword.ToLower();

                query = query.Where(p =>
                    p.ProductName.ToLower().Contains(keyword) ||

                    (p.Description != null &&
                     p.Description.ToLower().Contains(keyword)) ||

                    (p.Brand != null &&
                     p.Brand.ToLower().Contains(keyword)) ||

                    (p.SubCategory != null &&
                     p.SubCategory.SubCategoryName.ToLower().Contains(keyword)) ||

                    (p.SubCategory != null &&
                     p.SubCategory.Category != null &&
                     p.SubCategory.Category.CategoryName.ToLower().Contains(keyword))
                );
            }

            // Category Filter
            if (filter.CategoryId.HasValue)
            {
                query = query.Where(p =>
                    p.SubCategory!.CategoryId ==
                    filter.CategoryId.Value);
            }

            // SubCategory Filter
            if (filter.SubCategoryId.HasValue)
            {
                query = query.Where(p =>
                    p.SubCategoryId ==
                    filter.SubCategoryId.Value);
            }

            // Brand Filter
            if (!string.IsNullOrWhiteSpace(filter.Brand))
            {
                query = query.Where(p =>
                    p.Brand == filter.Brand);
            }

            // Min Price
            if (filter.MinPrice.HasValue)
            {
                query = query.Where(p =>
                    p.Price >= filter.MinPrice.Value);
            }

            // Max Price
            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(p =>
                    p.Price <= filter.MaxPrice.Value);
            }
            // Sorting
            switch (filter.SortBy)
            {
                case "priceAsc":
                    query = query.OrderBy(p => p.Price);
                    break;

                case "priceDesc":
                    query = query.OrderByDescending(p => p.Price);
                    break;

                case "name":
                    query = query.OrderBy(p => p.ProductName);
                    break;

                case "newest":
                    query = query.OrderByDescending(p => p.CreatedAt);
                    break;

                default:
                    query = query.OrderByDescending(p => p.ProductId);
                    break;
            }

            // Pagination
            var totalRecords =
      await query.CountAsync();

            query = query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize);

            var products =
                await query.ToListAsync();

            return new PagedResultDTO<ProductResponseDTO>
            {
                Items =
                    _mapper.Map<List<ProductResponseDTO>>(products),

                CurrentPage =
                    filter.Page,

                PageSize =
                    filter.PageSize,

                TotalRecords =
                    totalRecords,

                TotalPages =
                    (int)Math.Ceiling(
                        totalRecords /
                        (double)filter.PageSize)
            };
        }
        public async Task<ProductResponseDTO?> GetProductByIdAsync(int id)
        {
            var product = await _context.Products
    .Include(p => p.Seller)
    .Include(p => p.SubCategory)
        .ThenInclude(sc => sc.Category)
    .Include(p => p.Inventory)
   .FirstOrDefaultAsync(
    p => p.ProductId == id &&
         p.IsActive);

            return product == null? null: _mapper.Map<ProductResponseDTO>(product);
        }
        public async Task<List<ProductResponseDTO>> GetMyProductsAsync(int userId)
        {
            var products = await _context.Products
                .Include(p => p.Inventory)
                .Include(p => p.SubCategory)
                    .ThenInclude(sc => sc.Category)
                .Include(p => p.Seller)
                .Where(p => p.Seller!.UserId == userId && p.IsActive)
                .ToListAsync();

            return products.Select(p => new ProductResponseDTO
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
                QuantityAvailable =
                    p.Inventory != null
                        ? p.Inventory.QuantityAvailable
                        : 0,

                CategoryName =
                    p.SubCategory?.Category?.CategoryName,

                SubCategoryName =
                    p.SubCategory?.SubCategoryName,

                SellerName =
                    p.Seller?.StoreName
            }).ToList();
        }
        public async Task<IEnumerable<ProductResponseDTO>> GetRelatedProductsAsync(int productId)
        {
            // Get current product
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
                return new List<ProductResponseDTO>();

            // Get products from same subcategory
            var relatedProducts = await _context.Products
                .Include(p => p.SubCategory)
                .ThenInclude(sc => sc.Category)
                .Include(p => p.Inventory)
                .Where(p =>
                    p.ProductId != productId &&
                    p.SubCategoryId == product.SubCategoryId &&
                    p.IsActive)
                .Take(4)
                .ToListAsync();

            return _mapper.Map<List<ProductResponseDTO>>(relatedProducts);
        }
        public async Task<ProductResponseDTO> CreateProductAsync(int userId,ProductCreateDTO dto)
        {
            var seller = await _context.Sellers.FirstOrDefaultAsync(s => s.UserId == userId);

            if (seller == null)
                throw new Exception("Seller profile not found.");

            var product = _mapper.Map<Product>(dto);

            product.SellerId = seller.SellerId;
            product.CreatedAt = DateTime.Now;
            product.UpdatedAt = DateTime.Now;
            product.IsActive = true;

            _context.Products.Add(product);

            await _context.SaveChangesAsync();
            _logger.LogInformation(
    "Product {ProductName} created by User {UserId}",
    product.ProductName,
    userId);

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

            if (savedProduct == null)
                throw new Exception("Product not found after creation.");

            return _mapper.Map<ProductResponseDTO>(savedProduct);
        }
        public async Task<bool> UpdateProductAsync(int userId,int productId, ProductUpdateDTO dto)
        {
            var seller = await _context.Sellers.FirstOrDefaultAsync(s => s.UserId == userId);

            if (seller == null)
                return false;

            var product = await _context.Products
                .FirstOrDefaultAsync(p =>
                    p.ProductId == productId &&
                    p.SellerId == seller.SellerId && p.IsActive);

            if (product == null)
                return false;

      

            _mapper.Map(dto, product);
            product.IsActive = true;

            product.UpdatedAt = DateTime.Now;

            var inventory = await _context.Inventories
                .FirstOrDefaultAsync(i => i.ProductId == product.ProductId);

            if (inventory != null)
            {
                inventory.QuantityAvailable = dto.QuantityAvailable;
                inventory.LastUpdated = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation(
    "Product {ProductId} updated by User {UserId}",
    productId,
    userId);

            return true;
        }

        public async Task<bool> DeleteProductAsync(int userId,int productId)
        {
            var seller = await _context.Sellers.FirstOrDefaultAsync(s => s.UserId == userId);

            if (seller == null)
                return false;

            var product = await _context.Products.FirstOrDefaultAsync(p =>p.ProductId == productId &&p.SellerId == seller.SellerId && p.IsActive);

            if (product == null)
                return false;

            product.IsActive = false;
            product.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            _logger.LogInformation(
    "Product {ProductId} deleted by User {UserId}",
    productId,
    userId);

            return true;
        }
    }
}
