using Asp.Versioning;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuitQ.Data;
using QuitQ.Mappings;
using QuitQ.Middleware;
using QuitQ.Services.AddressService;
using QuitQ.Services.AdminFeature;
using QuitQ.Services.AuthFeature;
using QuitQ.Services.CartFeature;
using QuitQ.Services.CategoryFeature;
using QuitQ.Services.OrderFeature;
using QuitQ.Services.PaymentFeature;
using QuitQ.Services.ProductFeature;
using QuitQ.Services.SellerFeature;
using QuitQ.Services.SubCategoryFeature;
using QuitQ.Services.UserFeature;
using QuitQ.Services.AdminFeature;
using System.Text;
using QuitQ.Configurations;
using QuitQ.Services.EmailFeature;
using QuitQ.Services.InvoiceFeature;

var builder = WebApplication.CreateBuilder(args);

// =====================
// Database
// =====================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("constr")));

// =====================
// JWT Authentication
// =====================
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["Jwt:Key"]!))
        };
    });
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

// =====================
// Services (DI)
// =====================
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ISubCategoryService, SubCategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ISellerService, SellerService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<
    IEmailService,
    EmailService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();

// =====================
// Exception Handler
// =====================
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);

    options.AssumeDefaultVersionWhenUnspecified = true;

    options.ReportApiVersions = true;
});

// =====================
// Controllers
// =====================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact",
        policy =>
        {
            policy.AllowAnyOrigin()
      .AllowAnyHeader()
      .AllowAnyMethod();
        });
});
builder.Services.AddControllers();

// =====================
// Swagger with JWT Button
// =====================
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "QuitQ API",
        Version = "v1",
        Description = "QuitQ E-Commerce API"
    });

    // This adds the Authorize button
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token here. Example: eyJhbGci..."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

var app = builder.Build();

// =====================
// Middleware Pipeline
// =====================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "QuitQ API V1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseExceptionHandler();

app.UseHttpsRedirection();
app.UseCors("AllowReact");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();