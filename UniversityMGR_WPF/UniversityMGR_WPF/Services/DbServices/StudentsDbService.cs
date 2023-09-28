using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UniversityMGR_WPF.Data;
using UniversityMGR_WPF.Models;
using UniversityMGR_WPF.Services.DbServices.Base;

namespace UniversityMGR_WPF.Services.DbServices
{
    internal class StudentsDbService : DbServiceBase<Student>
    {
        private readonly Task10DbContext _db;

        public StudentsDbService(Task10DbContext db) : base(db) => _db = db;

        public override IQueryable<Student> Items => base.Items.Include(s => s.Group);

        public override async Task<Student> GetDetailAsync(int id, CancellationToken cancel = default)
        {
            var student = await base.Items
                .Include(s => s.Group)
                    .ThenInclude(g => g.Teacher)
                .Include(s => s.Group)
                    .ThenInclude(g => g.Course)
                .SingleOrDefaultAsync(s => s.Id == id, cancel)
                .ConfigureAwait(false);

            return student;
        }
    }
}
