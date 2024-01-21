using BLL.Interfaces;
using DAL.Entities;
using Newtonsoft.Json.Linq;

namespace BLL.Parsers;

public class JsonParser : IConfigParser
{
    public Tree Parse(string configFile, string fileName)
    {
        Tree resultTree = new();
        
        JToken configRoot = JToken.Parse(configFile);

        resultTree.Root = _traverseTree(configRoot, fileName, null);

        return resultTree;
    }

    private Node _traverseTree(JToken jNode, string fileName, Guid? parentId)
    {
        Node node = new()
        {
            Name = jNode.Path == "" 
                ? "root" 
                : jNode.Path.Split('.').Last(),
            ConfigName = fileName,
            ParentId = parentId
        };
        
        switch (jNode)
        {
            case JObject nonTerminal:
                foreach (var child in nonTerminal.Children<JProperty>())
                    node.Children.Add(_traverseTree(child.Value, fileName, node.Id));
                break;
            
            case JValue terminal:
                node.Value = terminal.Value?.ToString();
                break;
            
            default:
                throw new ArgumentException(nameof(jNode));
        }

        return node;
    }
}