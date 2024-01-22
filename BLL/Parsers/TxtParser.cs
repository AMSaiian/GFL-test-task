using BLL.Interfaces;
using DAL.Entities;

namespace BLL.Parsers;

public class TxtParser : IConfigParser
{
    public Tree Parse(string configFile, string fileName)
    {
        Tree resultTree = new()
        {
            Root = new Node()
            {
                Name = fileName
            }
        };
        
        using StringReader stringReader = new StringReader(configFile);
        while (stringReader.ReadLine() is { } line)
        {
            List<string> path = line.Split(":").ToList();
            
            _traverseTree(resultTree.Root, path);
        }

        return resultTree;
    }

    private void _traverseTree(Node parent, List<string> path)
    {
        if (path.Count > 2)
        {
            string nonTerminalName = path[0];
            path.RemoveAt(0);
            Node? nonTerminalNode = parent.Children.FirstOrDefault(x => x.Name == nonTerminalName);
            
            if (nonTerminalNode is not null)
            {
                _traverseTree(nonTerminalNode, path);
            }
            else
            {
                nonTerminalNode = new()
                {
                    Name = nonTerminalName
                };
                
                parent.Children.Add(nonTerminalNode);
                _traverseTree(nonTerminalNode, path);
            }
        }
        else
        {
            Node terminalNode = new()
            {
                Name = path[0],
                Value = path[1]
            };
            
            parent.Children.Add(terminalNode);
        }
    }
}