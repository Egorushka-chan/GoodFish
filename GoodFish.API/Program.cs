using System.Text.Json;
using System.Text.Json.Serialization;
using GoodFish.API.Middleware;
using GoodFish.DAL.DI;
using GoodFish.BLL.DI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
});

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.InjectDataAccessLayer(connectionString);
builder.Services.InjectBusinessLogicLayer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<CustomExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
