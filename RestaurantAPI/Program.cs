//utworzenie webhosta
using Microsoft.EntityFrameworkCore;
using RestaurantAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<RestaurantDbContext>
    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("RestaurantDbConnection"))); //dodanie kontekstu bazy
builder.Services.AddScoped<RestaurantSeeder>(); //dodanie serwisu do seedowania
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

//pobranie serwisu seeder z kontenera dependecy injection
// musimy najpierw utworzyc scope bo serwis jest typu scope
var scope = app.Services.CreateScope(); 
//teraz wydobywamy konkretny obiekt czyli seeder
var seeder = scope.ServiceProvider.GetRequiredService<RestaurantSeeder>();
//odpalenie metody seed serwisu seeder
seeder.Seed();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
