using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Task10.Data;
using Task10.Models;
using Task10.Services.DbServices.Base;

namespace Task10.Services.DbServices
{
    internal class GroupsDbService : DbServiceBase<Group>
    {
        private readonly Task10DbContext _db;

        public GroupsDbService(Task10DbContext db) : base(db) => _db = db;

        public override async Task<Group> GetDetailAsync(int id, CancellationToken cancel = default)
        {
            var group = await base.Items.Include(g => g.Students)
                .SingleOrDefaultAsync(g => g.Id == id, cancel)
                .ConfigureAwait(false);

            return group;
        }

        public override async Task RemoveAsync(int id, CancellationToken cancel = default)
        {
            var RemovableGroup = await GetDetailAsync(id, cancel);

            if (RemovableGroup is null)
                throw new InvalidOperationException($"Exeption! The {RemovableGroup} with ID={id} was not found in the context.");

            if (RemovableGroup.Students!.Count > 0)
                throw new DbUpdateException("You cannot delete a group with students");

            _db.Remove(RemovableGroup);
            await _db.SaveChangesAsync(cancel).ConfigureAwait(false);
        }
    }
}
