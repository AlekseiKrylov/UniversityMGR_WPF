using Microsoft.EntityFrameworkCore;
using Task10.Models;

namespace Task10.Data
{
    internal class Task10DbContext : DbContext
    {
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }

        public Task10DbContext(DbContextOptions<Task10DbContext> options) : base(options) { }
    }
}
