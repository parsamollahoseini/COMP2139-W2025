using COMP2139_ICE.Models;
using Microsoft.EntityFrameworkCore;

namespace COMP2139_ICE.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option)
    {
        
    }
    
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectTask> Tasks { get; set; }




    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //define one-to-many relationship
        
        //seeding the database
        modelBuilder.Entity<Project>().HasData(
            new Project { ProjectId = 1, Name = "Assignment 1", Description = "Comp2139 - Assignment 1" },
            new Project { ProjectId = 2, Name = "Assignment 2", Description = "Comp2139 - Assignment 2" }
);


    }


}