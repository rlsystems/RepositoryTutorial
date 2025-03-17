using Microsoft.EntityFrameworkCore;
using RepositoryTutorial.Infrastructure;
using RepositoryTutorial.Services.ProductService;

var builder = WebApplication.CreateBuilder(args);

// add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();

// register automapper
builder.Services.AddAutoMapper(typeof(MappingProfiles));

// register db context
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// register your repository
builder.Services.AddScoped<IRepositoryAsync, RepositoryAsync>();

// register app services
builder.Services.AddTransient<IProductService, ProductService>();

var app = builder.Build();

// configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// NOTE: To test the API use the postman collection included in this repo

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
