using BLL.Interfaces;
using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;

public class ConfigService : IConfigService
{
    private readonly IConfigParser _parser;

    private readonly TreeContext _context;

    public ConfigService(IConfigParser parser, TreeContext context)
    {
        _parser = parser;
        _context = context;
    }

    public async Task WriteConfigToDb(string configFile, string fileName)
    {
        if (await IsExist(fileName))
            throw new ArgumentException($"Configuration with name {fileName} already exists");

        Tree result = _parser.Parse(configFile, fileName);

        await _context.Set<Node>().AddAsync(result.Root!);

        await _context.SaveChangesAsync();
    }

    public async Task<Tree> RetrieveConfigFromDb(string fileName)
    {
        Tree result = new()
        {
            Root = await _context.Set<Node>()
                .Include(x =>
                    x.Children)
                .FirstOrDefaultAsync(x =>
                    x.Name == "root" &&
                    x.ConfigName == fileName)
        };

        return result;
    }

    private async Task<bool> IsExist(string fileName)
    {
        return await _context.Set<Node>().AnyAsync(x => x.ConfigName == fileName);
    }
}