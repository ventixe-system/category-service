using CategoryService.Api.Data;
using CategoryService.Api.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CategoryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

var app = builder.Build();
app.MapOpenApi();


app.UseHttpsRedirection();
app.UseCors("AllowAll");


app.MapGet("/api/categories", async (CategoryDbContext db) =>
{
    var categories = await db.Categories
        .Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name
        })
        .ToListAsync();

    return Results.Ok(categories);
});

app.MapGet("/api/categories/{id}", async (int id, CategoryDbContext db) =>
{
    var category = await db.Categories.FindAsync(id);
    return category is not null
        ? Results.Ok(new CategoryDto { Id = category.Id, Name = category.Name })
        : Results.NotFound();
});

app.MapPost("/api/categories", async (CategoryDto dto, CategoryDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(dto.Name))
    {
        return Results.BadRequest("Category name is required.");
    }

    var exists = await db.Categories.AnyAsync(c => c.Name.ToLower() == dto.Name.ToLower());
    if (exists)
    {
        return Results.Conflict("A category with that name already exists.");
    }

    var entity = new CategoryEntity
    {
        Name = dto.Name.Trim()
    };

    db.Categories.Add(entity);
    await db.SaveChangesAsync();

    var result = new CategoryDto
    {
        Id = entity.Id,
        Name = entity.Name
    };

    return Results.Created($"/api/categories/{result.Id}", result);
});

app.Run();
