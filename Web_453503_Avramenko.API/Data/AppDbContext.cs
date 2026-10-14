using Microsoft.EntityFrameworkCore;

namespace Web_453503_Avramenko.API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options)
{
    public DbSet<Species> Species { get; set; }
    public DbSet<Pet> Pets { get; set; }
}