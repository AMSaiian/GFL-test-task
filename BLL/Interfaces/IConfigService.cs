namespace BLL.Interfaces;

public interface IConfigService
{
    public Task WriteConfigToDb(string configFile, string fileName);

    public Task<Tree> RetrieveConfigFromDb(string fileName);
}