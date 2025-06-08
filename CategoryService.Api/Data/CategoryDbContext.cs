using Microsoft.EntityFrameworkCore;
using CategoryService.Api.Models;

namespace CategoryService.Api.Data;

public class CategoryDbContext : DbContext
{
    public CategoryDbContext(DbContextOptions<CategoryDbContext> options) : base(options) { }

    public DbSet<CategoryEntity> Categories => Set<CategoryEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CategoryEntity>().HasData(
            new CategoryEntity { Id = 1, Name = "Music" },
            new CategoryEntity { Id = 2, Name = "Outdoor & Adventure" },
            new CategoryEntity { Id = 3, Name = "Fashion" },
            new CategoryEntity { Id = 4, Name = "Food & Culinary" },
            new CategoryEntity { Id = 5, Name = "Art & Design" },
            new CategoryEntity { Id = 6, Name = "Sports" },
            new CategoryEntity { Id = 7, Name = "Technology" },
            new CategoryEntity { Id = 8, Name = "Health & Wellness" }
        );
    }
}


