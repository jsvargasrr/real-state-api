using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.DTOs;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class OwnersController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Controller for managing owner resources.
    /// </summary>
    public OwnersController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves all owners.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OwnerDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOwners(CancellationToken cancellationToken)
    {
        var owners = await _unitOfWork.Owners.GetAllAsync(cancellationToken);
        var dtos = _mapper.Map<IEnumerable<OwnerDto>>(owners);
        return Ok(dtos);
    }

    /// <summary>
    /// Retrieves an owner by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OwnerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOwner(Guid id, CancellationToken cancellationToken)
    {
        var owner = await _unitOfWork.Owners.GetByIdAsync(id, cancellationToken);

        if (owner == null)
            return NotFound(new { error = "Owner not found" });

        var dto = _mapper.Map<OwnerDto>(owner);
        return Ok(dto);
    }

    /// <summary>
    /// Creates a new owner.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(OwnerDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOwner([FromBody] CreateOwnerRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return BadRequest(new { error = "Name is required" });

        var owner = _mapper.Map<Owner>(request);
        await _unitOfWork.Owners.AddAsync(owner, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<OwnerDto>(owner);
        return CreatedAtAction(nameof(GetOwner), new { id = dto.IdOwner }, dto);
    }
}
