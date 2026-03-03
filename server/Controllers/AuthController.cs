using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using server.DTOs.auth;
using server.Repositories;
using server.Services;
using shared;
using shared.Domain;
using shared.DTOs;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public AuthController(
            TokenService tokenService,
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (existingUser != null)
                return Conflict("Email already exists");

            var newUser = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = UserRole.User,
                Phone = string.Empty,
                Avatar = string.Empty,
                Status = UserStatus.Pending,
                IsEmailVerified = false
            };

            await _userRepository.CreateNewUserAsync(newUser);

            var response = new UserResponseDto
            {
                Id = newUser.Id,
                Name = newUser.Name,
                Email = newUser.Email,
                Role = newUser.Role,
                Phone = newUser.Phone,
                Avatar = newUser.Avatar,
                Status = newUser.Status,
                IsEmailVerified = newUser.IsEmailVerified,
                CreatedAtUTC = newUser.CreatedAtUTC,
                UpdatedAtUTC = newUser.UpdatedAtUTC
            };

            return CreatedAtAction(
                nameof(Register),
                new { id = response.Id },
                response
            );
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (existingUser is null)
                return Unauthorized("Invalid email or password");

            var isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, existingUser.Password);
            if (!isPasswordValid)
                return Unauthorized("Invalid email or password");

            const int maxDevices = 3;

            int activeTokenCount = await _refreshTokenRepository.GetActiveTokenCountByUserIdAsync(existingUser.Id);
            if (activeTokenCount >= maxDevices)
                return StatusCode(429, $"Maximum {maxDevices} devices allowed. Please logout from another device");

            var (accessToken, jti) = _tokenService.CreateAccessToken(existingUser);
            var refreshToken = _tokenService.CreateRefreshToken(existingUser.Id, jti);

            await _refreshTokenRepository.CreateRefreshTokenAsync(refreshToken);

            return Ok(new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.RefreshToken
            });
        }


        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutUserDto dto)
        {
            var currentRefreshToken = await _refreshTokenRepository.GetByRefreshTokenAsync(dto.RefreshToken);
            if (currentRefreshToken is null)
                return NotFound("Refresh token not found");

            if (!currentRefreshToken.IsActive)
            {
                return Ok(new
                {
                    message = "Already logged out!"
                });
            }

            await _refreshTokenRepository.RevokeAsync(currentRefreshToken);

            return Ok(new
            {
                message = "Logged out successfully"
            });
        }


        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto dto)
        {
            var currentRefreshToken = await _refreshTokenRepository.GetByRefreshTokenAsync(dto.RefreshToken);
            if (currentRefreshToken is null)
                return Unauthorized("Invalid refresh token");

            if (!currentRefreshToken.IsActive)
                return Unauthorized("Refresh token has been revoked or expired");

            var currentUser = await _userRepository.GetUserByIdAsync(currentRefreshToken.UserId);
            if (currentUser is null)
                return Unauthorized("User not found");

            var (accessToken, jti) = _tokenService.CreateAccessToken(currentUser);
            var newRefreshToken = _tokenService.CreateRefreshToken(currentUser.Id, jti);

            await _refreshTokenRepository.CreateRefreshTokenAsync(newRefreshToken);

            currentRefreshToken.ReplacedByRefreshToken = newRefreshToken.RefreshToken;
            await _refreshTokenRepository.RevokeAsync(currentRefreshToken);

            return Ok(new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken.RefreshToken
            });
        }
    }
}
