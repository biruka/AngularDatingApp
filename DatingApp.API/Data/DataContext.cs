using Microsoft.EntityFrameworkCore;
using DatingApp.API.Models;
using System.Threading;

namespace DatingApp.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options){}

        public DbSet<Value> Values { get; set; }
    }
}