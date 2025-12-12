using FluentAssertions;
using RealEstate.Domain.Entities;

namespace RealEstate.Tests.Domain;

[TestFixture]
public class PropertyTests
{
    [Test]
    public void ChangePrice_WithValidPrice_ShouldUpdatePriceAndCreateTrace()
    {
        var property = new Property
        {
            IdProperty = Guid.NewGuid(),
            Name = "Test Property",
            Address = "123 Test St",
            Price = 100000,
            CodeInternal = "TEST-001",
            Year = 2020,
            IdOwner = Guid.NewGuid()
        };

        var newPrice = 150000m;

        property.ChangePrice(newPrice);

        property.Price.Should().Be(newPrice);
        property.PropertyTraces.Should().HaveCount(1);
        property.UpdatedAt.Should().NotBeNull();
    }

    [Test]
    public void ChangePrice_ShouldCreateTraceWithCorrectData()
    {
        var property = new Property
        {
            IdProperty = Guid.NewGuid(),
            Name = "Test Property",
            Address = "123 Test St",
            Price = 100000,
            CodeInternal = "TEST-001",
            Year = 2020,
            IdOwner = Guid.NewGuid()
        };

        var newPrice = 200000m;

        property.ChangePrice(newPrice);

        var trace = property.PropertyTraces.First();
        trace.IdProperty.Should().Be(property.IdProperty);
        trace.Value.Should().Be(newPrice);
        trace.Tax.Should().Be(newPrice * 0.05m);
        trace.Name.Should().Contain("100,000");
        trace.Name.Should().Contain("200,000");
    }

    [Test]
    public void ChangePrice_WithNegativePrice_ShouldThrowArgumentException()
    {
        var property = new Property
        {
            IdProperty = Guid.NewGuid(),
            Name = "Test Property",
            Address = "123 Test St",
            Price = 100000,
            CodeInternal = "TEST-001",
            Year = 2020,
            IdOwner = Guid.NewGuid()
        };

        Action act = () => property.ChangePrice(-100);

        act.Should().Throw<ArgumentException>()
           .WithMessage("*negative*");
    }

    [Test]
    public void ChangePrice_MultipleTimes_ShouldCreateMultipleTraces()
    {
        var property = new Property
        {
            IdProperty = Guid.NewGuid(),
            Name = "Test Property",
            Address = "123 Test St",
            Price = 100000,
            CodeInternal = "TEST-001",
            Year = 2020,
            IdOwner = Guid.NewGuid()
        };

        property.ChangePrice(150000);
        property.ChangePrice(200000);
        property.ChangePrice(180000);

        property.PropertyTraces.Should().HaveCount(3);
        property.Price.Should().Be(180000);
    }
}
