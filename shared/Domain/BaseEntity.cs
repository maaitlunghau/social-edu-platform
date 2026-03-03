namespace shared.Domain;

public class BaseEntity
{
    public DateTime CreatedAtUTC { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAtUTC { get; set; }
}
