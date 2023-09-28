using Microsoft.EntityFrameworkCore;
using UniversityMGR_WPF.Models;

namespace UniversityMGR_WPF.Data
{
    internal class UniversityMGRDbContext : DbContext
    {
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }

        public UniversityMGRDbContext(DbContextOptions<UniversityMGRDbContext> options) : base(options) { }
    }
}
