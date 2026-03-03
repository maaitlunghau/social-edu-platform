using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using shared.Domain;

namespace shared;

public class RefreshTokenRecords : BaseEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();


    [Required]
    public Guid UserId { get; set; }


    [Required]
    [StringLength(500)]
    public string RefreshToken { get; set; } = string.Empty;


    [Required]
    [StringLength(100)]
    public string AccessTokenJTI { get; set; } = string.Empty;


    [StringLength(500)]
    public string? ReplacedByRefreshToken { get; set; }


    public DateTime? RevokedAtUTC { get; set; }


    [Required]
    public DateTime ExpireAtUTC { get; set; }


    [NotMapped]
    public bool IsExpired => DateTime.UtcNow >= ExpireAtUTC;


    [NotMapped]
    public bool IsActive => !IsExpired && RevokedAtUTC == null;


    public User? User { get; set; }
}
