using GacorAPI.Data;
using GacorAPI.Data.Seed;
using GacorAPI.Data.Uow;
using GacorAPI.Domain.UserDom;
using GacorAPI.Infra;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Get configuration
var config = ConfigHelper.GetConfig();

// Add services to the container.
var mysqlVersion = new MySqlServerVersion(new Version(5,7));
builder.Services.AddDbContext<GacorContext>(options => options.UseMySql(config.ConnectionString, mysqlVersion));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Seed data
using(var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<GacorContext>();
    Seeder.SeedData(context);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
