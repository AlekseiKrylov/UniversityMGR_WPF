using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Task10.Data;
using Task10.Models.Base;
using Task10.Services.Interfaces;

namespace Task10.Services.Base
{
    internal class DbServiceBase<T> : IDbService<T> where T : Entity, new()
    {
        private readonly Task10DbContext _db;
        private readonly DbSet<T> _dbSet;

        public DbServiceBase(Task10DbContext db)
        {
            _db = db;
            _dbSet = _db.Set<T>();
        }

        public virtual IQueryable<T> Items => _dbSet;

        public virtual async Task<T> GetDetailAsync(int id, CancellationToken cancel = default)
        {
            T? item = await Items
                        .SingleOrDefaultAsync(item => item.Id == id, cancel)
                        .ConfigureAwait(false);

            return item;
        }

        public virtual async Task<T> AddAsync(T item, CancellationToken cancel = default)
        {
            if (item is null)
                throw new ArgumentNullException($"Exeption! Lost data. The object {nameof(item)} is null.");

            _db.Entry(item).State = EntityState.Added;
            await _db.SaveChangesAsync(cancel).ConfigureAwait(false);

            return item;
        }

        public virtual async Task UpdateAsync(T item, CancellationToken cancel = default)
        {
            if (item is null)
                throw new ArgumentNullException($"Exeption! Lost data. The object {nameof(item)} is null.");

            _db.Entry(item).State = EntityState.Modified;
            await _db.SaveChangesAsync(cancel).ConfigureAwait(false);
        }

        public virtual async Task RemoveAsync(int id, CancellationToken cancel = default)
        {
            var item = await GetDetailAsync(id, cancel);
            if (item is null)
                throw new InvalidOperationException($"Exeption! The {item} with ID={id} was not found in the context.");

            _db.Remove(item);
            await _db.SaveChangesAsync(cancel).ConfigureAwait(false);
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> items, CancellationToken cancel = default)
        {
            if (items is null)
                throw new ArgumentNullException($"Exception! Lost data. The {nameof(items)} collection is null.");

            await _dbSet.AddRangeAsync(items, cancel).ConfigureAwait(false);
            await _db.SaveChangesAsync(cancel).ConfigureAwait(false);
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<T> items, CancellationToken cancel = default)
        {
            if (items is null)
                throw new ArgumentNullException($"Exception! Lost data. The {nameof(items)} collection is null.");

            foreach (T item in items)
                _db.Entry(item).State = EntityState.Modified;
            await _db.SaveChangesAsync(cancel).ConfigureAwait(false);
        }
    }
}
