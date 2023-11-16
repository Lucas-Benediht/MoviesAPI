using FilmsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmsAPI.Data;

public class MovieContext : DbContext
{
   public MovieContext(DbContextOptions<MovieContext> opts)
        : base(opts) 
    {
        
    }
    public DbSet<Movie> Movies { get; set; }
}
