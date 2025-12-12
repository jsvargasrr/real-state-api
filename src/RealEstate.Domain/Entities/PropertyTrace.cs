namespace RealEstate.Domain.Entities;

public class PropertyTrace
{
    public Guid IdPropertyTrace { get; set; }
    public Guid IdProperty { get; set; }
    public DateTime DateSale { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public decimal Tax { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual Property Property { get; set; } = null!;
}
