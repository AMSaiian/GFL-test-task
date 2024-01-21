using BLL.Interfaces;
using BLL.Parsers;
using BLL.Services;

namespace PL;

public static class ConfigureServices
{
    public static IServiceCollection AddServices(this IServiceCollection self)
    {
        self.AddScoped<IConfigParser, JsonParser>();
        self.AddScoped<IConfigService, ConfigService>();

        return self;
    }
}