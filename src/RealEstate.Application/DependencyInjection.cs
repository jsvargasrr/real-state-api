using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RealEstate.Application.Mappings;
using RealEstate.Application.UseCases.AddPropertyImage;
using RealEstate.Application.UseCases.ChangePropertyPrice;
using RealEstate.Application.UseCases.CreateProperty;
using RealEstate.Application.UseCases.GetProperty;
using RealEstate.Application.UseCases.ListProperties;
using RealEstate.Application.UseCases.UpdateProperty;
using RealEstate.Application.UseCases.Reservations;

namespace RealEstate.Application;

/// <summary>
/// Registers application-level services, handlers, mappings, and validators.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds application services and configuration to the dependency injection container.
    /// </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile).Assembly);
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddScoped<CreatePropertyHandler>();
        services.AddScoped<GetPropertyHandler>();
        services.AddScoped<UpdatePropertyHandler>();
        services.AddScoped<ChangePropertyPriceHandler>();
        services.AddScoped<ListPropertiesHandler>();
        services.AddScoped<AddPropertyImageHandler>();

        services.AddScoped<CreateReservationHandler>();
        services.AddScoped<GetPropertyReservationsHandler>();
        services.AddScoped<CancelReservationHandler>();

        return services;
    }
}
