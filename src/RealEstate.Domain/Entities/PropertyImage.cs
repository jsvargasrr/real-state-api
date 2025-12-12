namespace RealEstate.Domain.Entities;

public class PropertyImage
{
    public Guid IdPropertyImage { get; set; }
    public Guid IdProperty { get; set; }
    public string File { get; set; } = string.Empty;
    public bool Enabled { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual Property Property { get; set; } = null!;
}
