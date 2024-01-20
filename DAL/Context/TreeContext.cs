using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context;

public class TreeContext(DbContextOptions<TreeContext> options) : DbContext(options)
{
    public required DbSet<Node> Nodes { get; set; }
}