using System.ComponentModel.DataAnnotations;

namespace server.DTOs.auth;

public class LoginUserDto
{
    [Required(ErrorMessage = "Email người dùng không phép bỏ trống!")]
    [StringLength(100, ErrorMessage = "Email người dùng không được phép quá 100 ký tự!")]
    [EmailAddress(ErrorMessage = "Email không đúng định dạng!")]
    public string Email { get; set; } = string.Empty;


    [Required(ErrorMessage = "Mật khẩu không được phép bỏ trống!")]
    [StringLength(100, ErrorMessage = "Mật khẩu không được phép quá 100 ký tự!")]
    public string Password { get; set; } = string.Empty;
}
