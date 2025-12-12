namespace RealEstate.Domain.Entities;

public class Property
{
    public Guid IdProperty { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string CodeInternal { get; set; } = string.Empty;
    public int Year { get; set; }
    public Guid IdOwner { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public virtual Owner Owner { get; set; } = null!;
    public virtual ICollection<PropertyImage> PropertyImages { get; set; } = new List<PropertyImage>();
    public virtual ICollection<PropertyTrace> PropertyTraces { get; set; } = new List<PropertyTrace>();
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public void ChangePrice(decimal newPrice)
    {
        if (newPrice < 0)
            throw new ArgumentException("Price cannot be negative", nameof(newPrice));

        var trace = new PropertyTrace
        {
            IdPropertyTrace = Guid.NewGuid(),
            IdProperty = IdProperty,
            DateSale = DateTime.UtcNow,
            Name = $"Price changed from {Price:C} to {newPrice:C}",
            Value = newPrice,
            Tax = CalculateTax(newPrice)
        };

        PropertyTraces.Add(trace);
        Price = newPrice;
        UpdatedAt = DateTime.UtcNow;
    }

    private static decimal CalculateTax(decimal price)
    {
        return price * 0.05m;
    }
}
