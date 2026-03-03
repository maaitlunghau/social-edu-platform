using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using shared.Domain;

namespace server.DTOs;

public class UpdateUserDto
{
    [StringLength(50, ErrorMessage = "Tên người dùng không được phép quá 50 ký tự!")]
    public string? Name { get; set; }


    [StringLength(100, ErrorMessage = "Email người dùng không được phép quá 100 ký tự!")]
    [EmailAddress(ErrorMessage = "Email không đúng định dạng!")]
    public string? Email { get; set; }


    [StringLength(100, ErrorMessage = "Mật khẩu không được phép quá 100 ký tự!")]
    public string? Password { get; set; }


    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserRole? Role { get; set; }


    [StringLength(255, ErrorMessage = "Avatar không được phép quá 255 ký tự!")]
    public string? Avatar { get; set; }


    [StringLength(20, ErrorMessage = "Số điện thoại không được phép quá 20 ký tự!")]
    public string? Phone { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserStatus? Status { get; set; }


    public bool? IsEmailVerified { get; set; }
}
