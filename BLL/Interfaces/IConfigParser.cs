namespace BLL.Interfaces;

public interface IConfigParser
{
    public Tree Parse(string configFile, string fileName);
}