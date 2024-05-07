//utworzenie webhosta
using System.Reflection;
using NLog.Web;
using RestaurantAPI;

var builder = WebApplication.CreateBuilder(args);

//NLog: Setup NLog for dependecy injection
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Host.UseNLog();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<RestaurantDbContext>(); //dodanie kontekstu bazy
builder.Services.AddScoped<RestaurantSeeder>(); //dodanie serwisu do seedowania
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly()); //rejestracja automapera
builder.Services.AddScoped<IRestaurantService, RestaurantService>(); //rejestracja serwisu Restaurant do kontrolera
builder.Services.AddScoped<IDishService, DishService>(); //rejestracja serwisu dish
builder.Services.AddScoped<ErrorHandlingMiddleware>(); //rejestracja ErrorHandlingMiddleware
builder.Services.AddScoped<RequestTimeMiddleware>(); //rejestracja RequestTimeMiddleware
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
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Restaurant API");
    });
}

//użyj middleware, kolejność ma znaczenie dlatego wołany jest jako pierwszy żeby mógł zacząć
//obsługiwać wyjątki po odpalaniu kolejnych elementó aplikacji
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestTimeMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
