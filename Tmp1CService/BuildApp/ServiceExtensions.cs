using Microsoft.EntityFrameworkCore;
using Tmp1CService.Models;
using Tmp1CService.Repositories;
using Tmp1CService.Repositories.Interfaces;
using Tmp1CService.Services;
using Tmp1CService.Services.Interfaces;

namespace Tmp1CService.BuildApp;

public static class ServiceExtensions
{
    public static void AddServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddHttpClient();
        builder.Services.AddScoped<IClientService, ClientService>();
        builder.Services.AddScoped<IClientRepository, ClientRepository>();
        
        builder.Services.AddScoped<IVehiclesService, VehiclesService>();
        builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
        
        builder.Services.AddScoped<IEmployeeService, EmployeeService>();
        builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        
        builder.Services.AddScoped<ITelematicsService, TelematicsService>();
        builder.Services.AddScoped<ITelematicsRepository, TelematicsRepository>();
        
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddAuthorization();
    }
}