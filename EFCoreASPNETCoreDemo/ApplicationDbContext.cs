using EFCoreASPNETCoreDemo.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCoreASPNETCoreDemo;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Person> People { get; set; }
}