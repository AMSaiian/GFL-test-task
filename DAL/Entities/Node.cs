using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities;

[Table(nameof(Node))]
public class Node
{
    [Key]
    public int? Id { get; set; }

    [Required(AllowEmptyStrings = false)]
    public required string Name { get; set; }
    
    public string? Value { get; set; }
    
    [ForeignKey("Parent")]
    public int? ParentId { get; set; }
    
    public Node? Parent { get; set; }
    
    public IList<Node> Children { get; set; } = new List<Node>();
}