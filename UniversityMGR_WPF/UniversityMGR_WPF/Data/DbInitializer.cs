using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace UniversityMGR_WPF.Data
{
    internal class DbInitializer
    {
        private readonly UniversityMGRDbContext _db;

        public DbInitializer(UniversityMGRDbContext db) => _db = db;

        public async Task DbInitializeAsync() => await _db.Database.MigrateAsync();
    }
}
