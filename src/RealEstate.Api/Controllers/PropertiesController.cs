using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.DTOs;
using RealEstate.Application.UseCases.AddPropertyImage;
using RealEstate.Application.UseCases.ChangePropertyPrice;
using RealEstate.Application.UseCases.CreateProperty;
using RealEstate.Application.UseCases.GetProperty;
using RealEstate.Application.UseCases.ListProperties;
using RealEstate.Application.UseCases.UpdateProperty;

namespace RealEstate.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PropertiesController : ControllerBase
{
    private readonly CreatePropertyHandler _createPropertyHandler;
    private readonly GetPropertyHandler _getPropertyHandler;
    private readonly UpdatePropertyHandler _updatePropertyHandler;
    private readonly ChangePropertyPriceHandler _changePriceHandler;
    private readonly ListPropertiesHandler _listPropertiesHandler;
    private readonly AddPropertyImageHandler _addImageHandler;

    /// <summary>
    /// Controller for managing properties.
    /// </summary>
    public PropertiesController(
        CreatePropertyHandler createPropertyHandler,
        GetPropertyHandler getPropertyHandler,
        UpdatePropertyHandler updatePropertyHandler,
        ChangePropertyPriceHandler changePriceHandler,
        ListPropertiesHandler listPropertiesHandler,
        AddPropertyImageHandler addImageHandler)
    {
        _createPropertyHandler = createPropertyHandler;
        _getPropertyHandler = getPropertyHandler;
        _updatePropertyHandler = updatePropertyHandler;
        _changePriceHandler = changePriceHandler;
        _listPropertiesHandler = listPropertiesHandler;
        _addImageHandler = addImageHandler;
    }

    /// <summary>
    /// Creates a property ensuring its owner exists and code is unique.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(PropertyDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProperty([FromBody] CreatePropertyRequest request, CancellationToken cancellationToken)
    {
        var result = await _createPropertyHandler.HandleAsync(request, cancellationToken);

        if (!result.IsSuccess)
        {
            return result.ErrorCode switch
            {
                "OWNER_NOT_FOUND" => NotFound(new { error = result.Error }),
                "DUPLICATE_CODE" => Conflict(new { error = result.Error }),
                _ => BadRequest(new { error = result.Error })
            };
        }

        return CreatedAtAction(nameof(GetPropertyById), new { id = result.Data!.IdProperty }, result.Data);
    }

    /// <summary>
    /// Updates an existing property.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(PropertyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateProperty(Guid id, [FromBody] UpdatePropertyRequest request, CancellationToken cancellationToken)
    {
        var result = await _updatePropertyHandler.HandleAsync(id, request, cancellationToken);

        if (!result.IsSuccess)
        {
            return result.ErrorCode switch
            {
                "NOT_FOUND" => NotFound(new { error = result.Error }),
                "OWNER_NOT_FOUND" => NotFound(new { error = result.Error }),
                "DUPLICATE_CODE" => Conflict(new { error = result.Error }),
                _ => BadRequest(new { error = result.Error })
            };
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Updates the price of a property.
    /// </summary>
    [HttpPatch("{id:guid}/price")]
    [ProducesResponseType(typeof(PropertyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangePrice(Guid id, [FromBody] ChangePriceRequest request, CancellationToken cancellationToken)
    {
        var result = await _changePriceHandler.HandleAsync(id, request, cancellationToken);

        if (!result.IsSuccess)
        {
            return result.ErrorCode switch
            {
                "NOT_FOUND" => NotFound(new { error = result.Error }),
                _ => BadRequest(new { error = result.Error })
            };
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Lists properties with filtering and pagination.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResult<PropertyDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProperties([FromQuery] PropertyFilterRequest request, CancellationToken cancellationToken)
    {
        var result = await _listPropertiesHandler.HandleAsync(request, cancellationToken);
        return Ok(result.Data);
    }

    /// <summary>
    /// Retrieves a property by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PropertyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPropertyById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _getPropertyHandler.HandleAsync(id, cancellationToken);

        if (!result.IsSuccess)
        {
            return NotFound(new { error = result.Error });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Adds an image to a property.
    /// </summary>
    [HttpPost("{id:guid}/images")]
    [ProducesResponseType(typeof(PropertyImageDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddImage(Guid id, [FromBody] AddPropertyImageRequest request, CancellationToken cancellationToken)
    {
        var result = await _addImageHandler.HandleAsync(id, request, cancellationToken);

        if (!result.IsSuccess)
        {
            return result.ErrorCode switch
            {
                "NOT_FOUND" => NotFound(new { error = result.Error }),
                _ => BadRequest(new { error = result.Error })
            };
        }

        return CreatedAtAction(nameof(GetPropertyById), new { id }, result.Data);
    }
}
