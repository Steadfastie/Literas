using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestsLiteras;

public static class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.Load("LiterasDataTransfer"));
    }
}
