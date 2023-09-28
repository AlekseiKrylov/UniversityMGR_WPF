using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using UniversityMGR_WPF.Data;
using UniversityMGR_WPF.Models;
using UniversityMGR_WPF.Services.DbServices.Base;

namespace UniversityMGR_WPF.Services.DbServices
{
    internal class TeachersDbService : DbServiceBase<Teacher>
    {
        private readonly UniversityMGRDbContext _db;

        public TeachersDbService(UniversityMGRDbContext db) : base(db) => _db = db;

        public override async Task<Teacher> GetDetailAsync(int id, CancellationToken cancel = default)
        {
            var teacher = await base.Items.Include(t => t.Groups)
                .SingleOrDefaultAsync(t => t.Id == id, cancel)
                .ConfigureAwait(false);

            return teacher;
        }

        public override async Task RemoveAsync(int id, CancellationToken cancel = default)
        {
            var RemovableTeacher = await GetDetailAsync(id, cancel);

            if (RemovableTeacher is null)
                throw new InvalidOperationException($"Exeption! The {RemovableTeacher} with ID={id} was not found in the context.");

            if (RemovableTeacher.Groups!.Count > 0)
                throw new DbUpdateException("You cannot delete a teacher with groups");

            _db.Remove(RemovableTeacher);
            await _db.SaveChangesAsync(cancel).ConfigureAwait(false);
        }
    }
}
