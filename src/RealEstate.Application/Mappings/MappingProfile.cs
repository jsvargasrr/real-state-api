using AutoMapper;
using RealEstate.Application.DTOs;
using RealEstate.Domain.Entities;

namespace RealEstate.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Owner, OwnerDto>();
        CreateMap<CreateOwnerRequest, Owner>()
            .ForMember(d => d.IdOwner, opt => opt.MapFrom(_ => Guid.NewGuid()));

        CreateMap<Property, PropertyDto>()
            .ForMember(d => d.OwnerName, opt => opt.MapFrom(s => s.Owner != null ? s.Owner.Name : null))
            .ForMember(d => d.Images, opt => opt.MapFrom(s => s.PropertyImages));

        CreateMap<CreatePropertyRequest, Property>()
            .ForMember(d => d.IdProperty, opt => opt.MapFrom(_ => Guid.NewGuid()));

        CreateMap<PropertyImage, PropertyImageDto>();
        CreateMap<AddPropertyImageRequest, PropertyImage>()
            .ForMember(d => d.IdPropertyImage, opt => opt.MapFrom(_ => Guid.NewGuid()));

        CreateMap<PropertyTrace, PropertyTraceDto>();

        CreateMap<Reservation, ReservationDto>();
    }
}
