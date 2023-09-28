using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace UniversityMGR_WPF.Data
{
    internal class DbInitializer
    {
        private readonly Task10DbContext _db;

        public DbInitializer(Task10DbContext db) => _db = db;

        public async Task DbInitializeAsync() => await _db.Database.MigrateAsync();
    }
}
