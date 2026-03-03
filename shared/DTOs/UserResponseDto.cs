using System.ComponentModel.DataAnnotations;
using shared.Domain;

namespace shared.DTOs;

public class UserResponseDto : BaseEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();


    [Required(ErrorMessage = "Tên người dùng không phép bỏ trống!")]
    [StringLength(50, ErrorMessage = "Tên người dùng không được phép quá 50 ký tự!")]
    public string Name { get; set; } = string.Empty;


    [Required(ErrorMessage = "Email người dùng không phép bỏ trống!")]
    [StringLength(100, ErrorMessage = "Email người dùng không được phép quá 100 ký tự!")]
    [EmailAddress(ErrorMessage = "Email không đúng định dạng!")]
    public string Email { get; set; } = string.Empty;


    [StringLength(20, ErrorMessage = "Vai trò người dùng không được phép quá 20 ký tự!")]
    public UserRole Role { get; set; } = UserRole.User;


    [StringLength(255, ErrorMessage = "Avatar không được phép quá 255 ký tự!")]
    public string Avatar { get; set; } = string.Empty;


    [StringLength(20, ErrorMessage = "Số điện thoại không được phép quá 20 ký tự!")]
    public string Phone { get; set; } = string.Empty;


    public UserStatus Status { get; set; } = UserStatus.Active;


    [Required(ErrorMessage = "Kiểm tra xác thực email không được phép bỏ trống!")]
    public bool IsEmailVerified { get; set; } = false;
}
