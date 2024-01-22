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
        if (!await IsExist(fileName))
            throw new ArgumentException($"Configuration with name {fileName} doesn't exists");
        
        Tree result = new()
        {
            Root = await _context.Set<Node>()
                .Include(x =>
                    x.Children)
                .FirstOrDefaultAsync(x =>
                    x.Name == fileName)
        };

        await LoadChildrenRecursively(result.Root.Children);

        return result;
    }

    private async Task<bool> IsExist(string fileName)
    {
        bool result = await _context.Set<Node>().AnyAsync(x => x.Name == fileName);
        return result;
    }
    
    private async Task LoadChildrenRecursively(ICollection<Node> chidren)
    {
        foreach (var node in chidren)
        {
            await _context.Entry(node)
                .Collection(x => x.Children)
                .LoadAsync();
            
            if (node.Children.Any())
            {
                await LoadChildrenRecursively(node.Children);
            }
        }
    }
}