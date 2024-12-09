using H4SoftwareTestOgSikkerhed.Models;
using Microsoft.EntityFrameworkCore;

namespace H4SoftwareTestOgSikkerhed.Data
{
    public class ToDoDBContext : DbContext
    {
        public ToDoDBContext(DbContextOptions<ToDoDBContext> options) : base(options)
        {
        }
        public DbSet<ToDoList> ToDoLists { get; set; }
        public DbSet<Cpr> Cprs { get; set; }
    }
}
