using App.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Photo> Photos { get; set; }
}
