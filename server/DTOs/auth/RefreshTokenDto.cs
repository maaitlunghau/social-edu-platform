using System;

namespace server.DTOs.auth;

public class RefreshTokenDto
{
    public string? RefreshToken { get; set; } = string.Empty;
}
