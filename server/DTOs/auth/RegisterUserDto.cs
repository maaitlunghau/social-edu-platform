using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using shared.Domain;

namespace server.DTOs.auth;

public class RegisterUserDto
{
    [Required(ErrorMessage = "Tên người dùng không phép bỏ trống!")]
    [StringLength(50, ErrorMessage = "Tên người dùng không được phép quá 50 ký tự!")]
    public string Name { get; set; } = string.Empty;


    [Required(ErrorMessage = "Email người dùng không phép bỏ trống!")]
    [StringLength(100, ErrorMessage = "Email người dùng không được phép quá 100 ký tự!")]
    [EmailAddress(ErrorMessage = "Email không đúng định dạng!")]
    public string Email { get; set; } = string.Empty;


    [Required(ErrorMessage = "Mật khẩu không được phép bỏ trống!")]
    [StringLength(100, ErrorMessage = "Mật khẩu không được phép quá 100 ký tự!")]
    public string Password { get; set; } = string.Empty;
}
