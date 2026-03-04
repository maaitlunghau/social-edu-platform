using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var secretKey = builder.Configuration["JWT:Key"]
                    ?? throw new InvalidOperationException("JWT:Key is not configured");
        var issuer = builder.Configuration["JWT:Issuer"]
            ?? throw new InvalidOperationException("JWT:Issuer is not configured");
        var audience = builder.Configuration["JWT:Audience"]
            ?? throw new InvalidOperationException("JWT:Audience is not configured");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                var jti = context.Principal?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
                if (string.IsNullOrEmpty(jti))
                {
                    context.Fail("Missing JTI claim in token");
                    return;
                }
                var db = context.HttpContext.RequestServices.GetRequiredService<DataContext>();

                var refreshToken = await db.RefreshTokenRecords.FirstOrDefaultAsync(
                    rft => rft.AccessTokenJTI == jti
                );
                if (refreshToken != null && !refreshToken.IsActive)
                {
                    context.Fail("Token has revoked or expired!");
                    return;
                }
            }
        };
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();
