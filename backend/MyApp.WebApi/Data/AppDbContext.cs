using Microsoft.EntityFrameworkCore;
using MyApp.WebApi.Models;
namespace MyApp.WebApi.Data;
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Report> Reports => Set<Report>();
}
