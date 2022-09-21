using EFCoreConsoleDemoApp.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreConsoleDemoApp;

internal class ApplicationDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=.;Database=EFCoreConsoleAppDemoDB;Integrated Security=True"); //integrated security=true means Windows credentials will be used        
    }

    public DbSet<Person> People { get; set; }
}