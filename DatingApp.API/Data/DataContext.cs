using DatingApp.API.models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DataContext : DbContext
    {

        // Constructor - Make it public, and specify options
        // - it also calls into the class we deriving from and also call its options.
        public DataContext(DbContextOptions<DataContext> options) : base(options){}

        
        // Property (Type of DbSet... value is type.)
        // Tell DataContext class about entities
        // Name of method is the name of table when scaffolding DB
        public DbSet<Value> Values { get; set; }

    }
}