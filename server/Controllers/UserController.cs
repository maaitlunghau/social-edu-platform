using Microsoft.AspNetCore.Mvc;
using server.DTOs;
using server.Repositories;
using shared;
using shared.DTOs;

namespace server.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
            => _userRepository = userRepository;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _userRepository.GetAllUserAsync();

            var response = users.Select(u => new UserResponseDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role,
                Phone = u.Phone,
                Avatar = u.Avatar,
                Status = u.Status,
                IsEmailVerified = u.IsEmailVerified,
                CreatedAtUTC = u.CreatedAtUTC,
                UpdatedAtUTC = u.UpdatedAtUTC
            });

            return Ok(response);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user is null) return NotFound($"User with {id} was not found");

            var response = new UserResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Phone = user.Phone,
                Avatar = user.Avatar,
                Status = user.Status,
                IsEmailVerified = user.IsEmailVerified,
                CreatedAtUTC = user.CreatedAtUTC,
                UpdatedAtUTC = user.UpdatedAtUTC
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (existingUser is not null)
                return Conflict($"Email {dto.Email} already exists");

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Role = dto.Role,
                Phone = dto.Phone,
                Avatar = dto.Avatar,
                Status = dto.Status,
                IsEmailVerified = dto.IsEmailVerified
            };

            await _userRepository.CreateNewUserAsync(user);

            var response = new UserResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Phone = user.Phone,
                Avatar = user.Avatar,
                Status = user.Status,
                IsEmailVerified = user.IsEmailVerified,
                CreatedAtUTC = user.CreatedAtUTC,
                UpdatedAtUTC = user.UpdatedAtUTC
            };

            return CreatedAtAction(
                nameof(GetById),
                new { id = response.Id },
                response
            );
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update([FromBody] UpdateUserDto dto, Guid id)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser is null)
                return NotFound($"User with ID {id} was not found");

            if (!string.IsNullOrEmpty(dto.Email) && dto.Email != existingUser.Email)
            {
                var emailExist = await _userRepository.GetUserByEmailAsync(dto.Email);
                if (emailExist is not null)
                    return Conflict("Email already exists");
            }

            existingUser.Name = dto.Name ?? existingUser.Name;
            existingUser.Email = dto.Email ?? existingUser.Email;
            existingUser.Role = dto.Role ?? existingUser.Role;
            existingUser.Phone = dto.Phone ?? existingUser.Phone;
            existingUser.Avatar = dto.Avatar ?? existingUser.Avatar;
            existingUser.Status = dto.Status ?? existingUser.Status;
            existingUser.IsEmailVerified = dto.IsEmailVerified ?? existingUser.IsEmailVerified;

            await _userRepository.UpdateUserAsync(existingUser);

            var response = new UserResponseDto
            {
                Id = existingUser.Id,
                Name = existingUser.Name,
                Email = existingUser.Email,
                Role = existingUser.Role,
                Phone = existingUser.Phone,
                Avatar = existingUser.Avatar,
                Status = existingUser.Status,
                IsEmailVerified = existingUser.IsEmailVerified,
                CreatedAtUTC = existingUser.CreatedAtUTC,
                UpdatedAtUTC = existingUser.UpdatedAtUTC
            };

            return Ok(response);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser is null)
                return NotFound($"User with ID {id} was not found");

            await _userRepository.DeleteUserAsync(existingUser);

            return NoContent();
        }
    }
}
