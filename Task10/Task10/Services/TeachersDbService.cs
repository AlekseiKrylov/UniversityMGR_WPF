using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Task10.Data;
using Task10.Models;
using Task10.Services.Base;

namespace Task10.Services
{
    internal class TeachersDbService : DbServiceBase<Teacher>
    {
        private readonly Task10DbContext _db;

        public TeachersDbService(Task10DbContext db) : base(db)
        {
            _db = db;
        }

        public override async Task<Teacher> GetAsync(int id, CancellationToken cancel = default)
        {
            var teacher = await base.Items.Include(t => t.Groups)
                .SingleOrDefaultAsync(t => t.Id == id, cancel)
                .ConfigureAwait(false);

            return teacher;
        }

        public override async Task RemoveAsync(int id, CancellationToken cancel = default)
        {
            var RemovableTeacher = await GetAsync(id, cancel);

            if (RemovableTeacher is null)
                throw new InvalidOperationException($"Exeption! The {RemovableTeacher} with ID={id} was not found in the context.");

            if (RemovableTeacher.Groups!.Count > 0)
                throw new DbUpdateException("You cannot delete a teacher with groups");

            _db.Remove(RemovableTeacher);
            await _db.SaveChangesAsync(cancel).ConfigureAwait(false);
        }
    }
}
