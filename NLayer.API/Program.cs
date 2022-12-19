using Microsoft.EntityFrameworkCore;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWork;
using NLayer.Repository;
using NLayer.Repository.Repositories;
using NLayer.Repository.UnitOfWork;
using NLayer.Service;
using NLayer.Service.Mapping;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(IGenericRepository<>));
builder.Services.AddScoped(typeof(IProductRepository), typeof(ProductRepository));
builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));
builder.Services.AddAutoMapper(typeof(MapProfile));
builder.Services.AddScoped(typeof(IProductService), typeof(ProductService));

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), opt =>
    {
        opt.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
