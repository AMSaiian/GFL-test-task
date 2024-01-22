using BLL.Interfaces;
using BLL.Parsers;
using BLL.Services;
using DAL.Context;

namespace PL;

public static class ConfigureServices
{
    public static IServiceCollection AddServices(this IServiceCollection self)
    {
        self.AddScoped<IConfigParser, JsonParser>();
        self.AddScoped<IConfigService, ConfigService>();

        return self;
    }

    public static async Task<WebApplication> EnsureCreateDb(this WebApplication self)
    {
        using var scope = self.Services.GetService<IServiceScopeFactory>().CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<TreeContext>();
        await context.Database.EnsureCreatedAsync();
        
        return self;
    }
}