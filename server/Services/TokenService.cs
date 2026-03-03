using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using shared;


namespace server.Services;

public class TokenService
{
    private readonly IConfiguration _config;
    public TokenService(IConfiguration config) => _config = config;

    public (string accessToken, string accessTokenJti) CreateAccessToken(User u)
    {
        var jti = Guid.NewGuid().ToString();

        if (string.IsNullOrEmpty(u.Email))
            throw new ArgumentException("Email is required", nameof(u.Email));
        if (string.IsNullOrEmpty(u.Role.ToString()))
            throw new ArgumentException("Role is required", nameof(u.Role));

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, u.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, u.Email),
            new(ClaimTypes.Role, u.Role.ToString()),
            new(JwtRegisteredClaimNames.Jti, jti),
        };

        var keyPlain = _config["JWT:Key"]
            ?? throw new InvalidOperationException("JWT Key is not set");
        var issure = _config["JWT:Issuer"]
            ?? throw new InvalidOperationException("JWT Issuer is not set");
        var audience = _config["JWT:Audience"]
            ?? throw new InvalidOperationException("JWT Audience is not set");
        var accessTokenMinutes = int.Parse(_config["JWT:AccessTokenMinutes"]
            ?? "5");

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyPlain));
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var now = DateTime.UtcNow;
        var jwtToken = new JwtSecurityToken(
            claims: claims,
            issuer: issure,
            audience: audience,
            signingCredentials: signingCredentials,
            expires: now.AddMinutes(accessTokenMinutes),
            notBefore: now
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        return (accessToken, jti);
    }

    public RefreshTokenRecords CreateRefreshToken(Guid userId, string accessTokenJti)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("User ID is required", nameof(userId));
        if (string.IsNullOrEmpty(accessTokenJti))
            throw new ArgumentException("Access token JTI is required", nameof(accessTokenJti));

        int days = int.Parse(_config["JWT:RefreshTokenDays"] ?? "0");
        if (days <= 0)
            throw new InvalidOperationException("JWT RefreshTokenDays must be greater than 0!");

        var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        var now = DateTime.UtcNow;
        return new RefreshTokenRecords
        {
            UserId = userId,
            RefreshToken = refreshToken,
            AccessTokenJTI = accessTokenJti,
            ExpireAtUTC = now.AddDays(days)
        };
    }
}
