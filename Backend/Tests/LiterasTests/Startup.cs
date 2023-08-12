using LiterasCore.Services;
using LiterasData.DTO;
using LiterasWebAPI.Config.MappingProfiles;
using Microsoft.Extensions.DependencyInjection;

namespace TestsLiteras;

public static class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(DocDto).Assembly);
        services.AddAutoMapper(typeof(DocsService).Assembly);
        services.AddAutoMapper(typeof(DocsProfile).Assembly);
    }
}