using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace WebApplication2.Endpoint;

public static class EndpointExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services)
    {
        services.AddEndpoints(Assembly.GetEntryAssembly());
        return services;
    }

    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        ServiceDescriptor[] serviceDescriptors = assembly
            .DefinedTypes
            .Where(type => type is {IsAbstract:false} && type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();
        
        services.TryAddEnumerable(serviceDescriptors);
        return services;
    }

    public static IApplicationBuilder MapEndpoints(this WebApplication app,
        RouteGroupBuilder? routeGroupBuilder = null)
    {
        var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();
        IEndpointRouteBuilder endpointRouteBuilder = routeGroupBuilder is null ? app : routeGroupBuilder;
        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(endpointRouteBuilder);
        }
        return app;
    }
}