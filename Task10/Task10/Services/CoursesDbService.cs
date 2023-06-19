using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Task10.Data;
using Task10.Models;
using Task10.Services.Base;

namespace Task10.Services
{
    internal class CoursesDbService : DbServiceBase<Course>
    {
        private readonly Task10DbContext _db;

        public CoursesDbService(Task10DbContext db) : base(db) => _db = db;

        public override async Task<Course> GetAsync(int id, CancellationToken cancel = default)
        {
            var course = await base.Items.Include(c => c.Groups)
                .SingleOrDefaultAsync(c => c.Id == id, cancel)
                .ConfigureAwait(false);

            return course;
        }

        public override async Task RemoveAsync(int id, CancellationToken cancel = default)
        {
            var RemovableCourse = await GetAsync(id, cancel);

            if (RemovableCourse is null)
                throw new InvalidOperationException($"Exeption! The {RemovableCourse} with ID={id} was not found in the context.");

            if (RemovableCourse.Groups!.Count > 0)
                throw new DbUpdateException("You cannot delete a course with groups");

            _db.Remove(RemovableCourse);
            await _db.SaveChangesAsync(cancel).ConfigureAwait(false);
        }
    }
}
