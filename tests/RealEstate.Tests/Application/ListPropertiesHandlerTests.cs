using AutoMapper;
using FluentAssertions;
using Moq;
using RealEstate.Application.DTOs;
using RealEstate.Application.Mappings;
using RealEstate.Application.UseCases.ListProperties;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Tests.Application;

[TestFixture]
public class ListPropertiesHandlerTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private IMapper _mapper;
    private ListPropertiesHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();

        _handler = new ListPropertiesHandler(_unitOfWorkMock.Object, _mapper);
    }

    [Test]
    public async Task HandleAsync_WithNoFilters_ShouldReturnAllProperties()
    {
        var request = new PropertyFilterRequest();
        var ownerId = Guid.NewGuid();

        var properties = new List<Property>
        {
            new()
            {
                IdProperty = Guid.NewGuid(),
                Name = "Property 1",
                Address = "Address 1",
                Price = 100000,
                CodeInternal = "P1",
                Year = 2020,
                IdOwner = ownerId,
                Owner = new Owner { IdOwner = ownerId, Name = "Owner 1", Address = "Test", Birthday = DateTime.Now }
            },
            new()
            {
                IdProperty = Guid.NewGuid(),
                Name = "Property 2",
                Address = "Address 2",
                Price = 200000,
                CodeInternal = "P2",
                Year = 2021,
                IdOwner = ownerId,
                Owner = new Owner { IdOwner = ownerId, Name = "Owner 1", Address = "Test", Birthday = DateTime.Now }
            }
        };

        _unitOfWorkMock
            .Setup(u => u.Properties.GetAllWithFiltersAsync(
                null, null, null, null, null, null, 1, 10, It.IsAny<CancellationToken>()))
            .ReturnsAsync((properties.AsEnumerable(), 2));

        var result = await _handler.HandleAsync(request);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Items.Should().HaveCount(2);
        result.Data.TotalCount.Should().Be(2);
    }

    [Test]
    public async Task HandleAsync_WithPriceFilter_ShouldReturnFilteredProperties()
    {
        var request = new PropertyFilterRequest(MinPrice: 150000, MaxPrice: 300000);
        var ownerId = Guid.NewGuid();

        var properties = new List<Property>
        {
            new()
            {
                IdProperty = Guid.NewGuid(),
                Name = "Mid-range Property",
                Address = "Address",
                Price = 200000,
                CodeInternal = "P2",
                Year = 2021,
                IdOwner = ownerId,
                Owner = new Owner { IdOwner = ownerId, Name = "Owner", Address = "Test", Birthday = DateTime.Now }
            }
        };

        _unitOfWorkMock
            .Setup(u => u.Properties.GetAllWithFiltersAsync(
                null, null, 150000m, 300000m, null, null, 1, 10, It.IsAny<CancellationToken>()))
            .ReturnsAsync((properties.AsEnumerable(), 1));

        var result = await _handler.HandleAsync(request);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Items.Should().HaveCount(1);
        result.Data.Items.First().Price.Should().Be(200000);
    }

    [Test]
    public async Task HandleAsync_WithPagination_ShouldReturnCorrectPage()
    {
        var request = new PropertyFilterRequest(Page: 2, PageSize: 5);

        _unitOfWorkMock
            .Setup(u => u.Properties.GetAllWithFiltersAsync(
                null, null, null, null, null, null, 2, 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Enumerable.Empty<Property>(), 10));

        var result = await _handler.HandleAsync(request);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Page.Should().Be(2);
        result.Data.PageSize.Should().Be(5);
        result.Data.TotalCount.Should().Be(10);
        result.Data.TotalPages.Should().Be(2);
    }
}
