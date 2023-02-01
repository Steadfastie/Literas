using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace TestsLiteras;

public static class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.Load("LiterasDataTransfer"));
    }
}