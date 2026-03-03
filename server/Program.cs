using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Filters;
using server.Repositories;
using server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers(options =>
    {
        options.Filters.Add<GlobalExceptionFilter>();
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter()
        );
    });

builder.Services.AddDbContext<DataContext>(option =>
{
    option.UseMySql(
        builder.Configuration.GetConnectionString("Connected"),
        new MySqlServerVersion(new Version(8, 0, 40))
    );
});

// register DI container
builder.Services.AddScoped<IUserRepository, UserService>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenService>();
builder.Services.AddSingleton<TokenService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();
