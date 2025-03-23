using Tmp1CService.BuildApp;

namespace Tmp1CService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ServiceExtensions.AddServices(builder);
        AuthenticationExtensions.AddApiKeyAuthentication(builder);
        Swagger.AddSecurity(builder);

        var app = builder.Build();
        Swagger.AddSwagger(app);
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}