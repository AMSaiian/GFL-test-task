using BLL.Interfaces;
using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Services;

public class ConfigService : IConfigService
{
    private readonly IConfigParser _jsonParser;
    
    private readonly IConfigParser _txtParser;

    private readonly TreeContext _context;

    public ConfigService(
        [FromKeyedServices("json")] IConfigParser jsonParser, 
        [FromKeyedServices("txt")] IConfigParser txtParser, 
        TreeContext context)
    {
        _jsonParser = jsonParser;
        _txtParser = txtParser;
        _context = context;
    }

    public async Task WriteConfigToDb(string configFile, string fileName)
    {
        if (await IsExist(fileName))
            throw new ArgumentException($"Configuration with name {fileName} already exists");

        string fileExtension = Path.GetExtension(fileName);

        Tree result;
        
        if (fileExtension == ".json")
            result = _jsonParser.Parse(configFile, fileName);
        else if (fileExtension == ".txt")
            result = _txtParser.Parse(configFile, fileName);
        else
            throw new ArgumentException($"Unsupportable file type");

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