using Microsoft.EntityFrameworkCore;
using QuitQ.Models;

namespace QuitQ.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {
        }

        public DbSet<Role> Roles { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Seller> Sellers { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<SubCategory> SubCategories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Inventory> Inventories { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(r => r.RoleId);
                entity.Property(r => r.RoleName)
                      .IsRequired()
                      .HasMaxLength(50);
                entity.HasIndex(r => r.RoleName).IsUnique();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UserId);
                entity.Property(u => u.Name)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(u => u.Email)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.PasswordHash)
                      .IsRequired()
                      .HasMaxLength(255);
                entity.Property(u => u.Phone)
                      .HasMaxLength(15);
                entity.Property(u => u.Gender)
                      .HasMaxLength(10);
                entity.Property(u => u.IsActive)
                      .HasDefaultValue(true);
                entity.Property(u => u.CreatedAt)
                      .HasDefaultValueSql("GETDATE()");

              
                entity.HasOne(u => u.Role)
                      .WithMany(r => r.Users)
                      .HasForeignKey(u => u.RoleId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

           
            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(a => a.AddressId);
                entity.Property(a => a.FullAddress)
                      .IsRequired()
                      .HasMaxLength(255);
                entity.Property(a => a.City)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(a => a.State)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(a => a.Pincode)
                      .IsRequired()
                      .HasMaxLength(10);
                entity.Property(a => a.Country)
                      .IsRequired()
                      .HasMaxLength(100);

                // Address -> User (Many to One)
                entity.HasOne(a => a.User)
                      .WithMany(u => u.Addresses)
                      .HasForeignKey(a => a.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Seller>(entity =>
            {
                entity.HasKey(s => s.SellerId);
                entity.Property(s => s.StoreName)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(s => s.GSTNumber)
                      .HasMaxLength(50);
                entity.Property(s => s.BusinessEmail)
                      .HasMaxLength(100);
                entity.Property(s => s.AccountHolderName)
                      .HasMaxLength(100);
                entity.Property(s => s.AccountNumber)
                      .HasMaxLength(30);
                entity.Property(s => s.IFSCCode)
                      .HasMaxLength(20);
                entity.Property(s => s.BankName)
                      .HasMaxLength(100);
                entity.HasIndex(s => s.UserId).IsUnique();

                // Seller -> User (One to One)
                entity.HasOne(s => s.User)
                      .WithOne(u => u.Seller)
                      .HasForeignKey<Seller>(s => s.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.CategoryId);
                entity.Property(c => c.CategoryName)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.HasIndex(c => c.CategoryName).IsUnique();
                entity.Property(c => c.Description)
                      .HasMaxLength(255);
            });

           
            modelBuilder.Entity<SubCategory>(entity =>
            {
                entity.HasKey(sc => sc.SubCategoryId);
                entity.Property(sc => sc.SubCategoryName)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(sc => sc.Description)
                      .HasMaxLength(255);

                // SubCategory -> Category (Many to One)
                entity.HasOne(sc => sc.Category)
                      .WithMany(c => c.SubCategories)
                      .HasForeignKey(sc => sc.CategoryId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

          
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.ProductId);
                entity.Property(p => p.ProductName)
                      .IsRequired()
                      .HasMaxLength(150);
                entity.Property(p => p.Description)
                      .HasMaxLength(1000);
                entity.Property(p => p.Price)
                      .HasColumnType("decimal(10,2)");
                entity.Property(p => p.Brand)
                      .HasMaxLength(100);
                entity.Property(p => p.ImageUrl)
                      .HasMaxLength(255);
                entity.Property(p => p.IsActive)
                      .HasDefaultValue(true);
                entity.Property(p => p.CreatedAt)
                      .HasDefaultValueSql("GETDATE()");

                // Product -> Seller (Many to One)
                entity.HasOne(p => p.Seller)
                      .WithMany(s => s.Products)
                      .HasForeignKey(p => p.SellerId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Product -> SubCategory (Many to One)
                entity.HasOne(p => p.SubCategory)
                      .WithMany(sc => sc.Products)
                      .HasForeignKey(p => p.SubCategoryId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.HasKey(i => i.InventoryId);
                entity.Property(i => i.QuantityAvailable)
                      .IsRequired();
                entity.Property(i => i.LastUpdated)
                      .HasDefaultValueSql("GETDATE()");
                entity.HasIndex(i => i.ProductId).IsUnique();

                // Inventory -> Product (One to One)
                entity.HasOne(i => i.Product)
                      .WithOne(p => p.Inventory)
                      .HasForeignKey<Inventory>(i => i.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(c => c.CartId);
                entity.Property(c => c.CreatedAt)
                      .HasDefaultValueSql("GETDATE()");
                entity.HasIndex(c => c.UserId).IsUnique();

                // Cart -> User (One to One)
                entity.HasOne(c => c.User)
                      .WithOne(u => u.Cart)
                      .HasForeignKey<Cart>(c => c.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

         
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(ci => ci.CartItemId);
                entity.Property(ci => ci.Quantity)
                      .IsRequired();

                // Unique: same product can't appear twice in same cart
                entity.HasIndex(ci => new { ci.CartId, ci.ProductId }).IsUnique();

                // CartItem -> Cart (Many to One)
                entity.HasOne(ci => ci.Cart)
                      .WithMany(c => c.CartItems)
                      .HasForeignKey(ci => ci.CartId)
                      .OnDelete(DeleteBehavior.Cascade);

                // CartItem -> Product (Many to One)
                entity.HasOne(ci => ci.Product)
                      .WithMany(p => p.CartItems)
                      .HasForeignKey(ci => ci.ProductId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

          
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.OrderId);
                entity.Property(o => o.OrderStatus)
                      .IsRequired()
                      .HasMaxLength(50)
                      .HasDefaultValue("Pending");
                entity.Property(o => o.TotalAmount)
                      .HasColumnType("decimal(10,2)");
                entity.Property(o => o.OrderDate)
                      .HasDefaultValueSql("GETDATE()");

                // Order -> User (Many to One)
                entity.HasOne(o => o.User)
                      .WithMany(u => u.Orders)
                      .HasForeignKey(o => o.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Order -> Address (Many to One)
                entity.HasOne(o => o.Address)
                      .WithMany(a => a.Orders)
                      .HasForeignKey(o => o.AddressId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(oi => oi.OrderItemId);
                entity.Property(oi => oi.Quantity)
                      .IsRequired();
                entity.Property(oi => oi.PriceAtPurchase)
                      .HasColumnType("decimal(10,2)");

                // OrderItem -> Order (Many to One)
                entity.HasOne(oi => oi.Order)
                      .WithMany(o => o.OrderItems)
                      .HasForeignKey(oi => oi.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);

                // OrderItem -> Product (Many to One)
                entity.HasOne(oi => oi.Product)
                      .WithMany(p => p.OrderItems)
                      .HasForeignKey(oi => oi.ProductId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(p => p.PaymentId);
                entity.Property(p => p.PaymentMethod)
                      .IsRequired()
                      .HasMaxLength(50);
                entity.Property(p => p.Amount)
                      .HasColumnType("decimal(10,2)");
                entity.Property(p => p.PaymentStatus)
                      .IsRequired()
                      .HasMaxLength(50)
                      .HasDefaultValue("Pending");
                entity.Property(p => p.TransactionId)
                      .HasMaxLength(100);
                entity.Property(p => p.PaymentDate)
                      .HasDefaultValueSql("GETDATE()");

                // Payment -> Order (Many to One)
                entity.HasOne(p => p.Order)
                      .WithMany(o => o.Payments)
                      .HasForeignKey(p => p.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(r => r.ReviewId);

                entity.Property(r => r.Rating)
                      .IsRequired();

                entity.Property(r => r.Comment)
                      .HasMaxLength(500);

                entity.Property(r => r.CreatedAt)
                      .HasDefaultValueSql("GETDATE()");

                entity.Property(r => r.IsActive)
                      .HasDefaultValue(true);

                // Review -> User
                entity.HasOne(r => r.User)
                      .WithMany(u => u.Reviews)
                      .HasForeignKey(r => r.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Review -> Product
                entity.HasOne(r => r.Product)
                      .WithMany(p => p.Reviews)
                      .HasForeignKey(r => r.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);

                // One review per user per product
                entity.HasIndex(r =>
                    new { r.UserId, r.ProductId })
                    .IsUnique();
            });

            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "Admin" },
                new Role { RoleId = 2, RoleName = "Seller" },
                new Role { RoleId = 3, RoleName = "Customer" }
            );
        }
    }

    }
