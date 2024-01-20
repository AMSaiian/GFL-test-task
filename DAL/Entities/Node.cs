using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities;

[Table(nameof(Node))]
public class Node
{
    [Key]
    [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.None)]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required(AllowEmptyStrings = false)]
    public required string ConfigName { get; set; }

    [Required(AllowEmptyStrings = false)]
    public required string Name { get; set; }
    
    public string? Value { get; set; }
    
    [ForeignKey("Parent")]
    public Guid? ParentId { get; set; }
    
    public Node? Parent { get; set; }
    
    public IList<Node> Children { get; set; } = new List<Node>();
}