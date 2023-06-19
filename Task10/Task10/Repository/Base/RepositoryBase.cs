using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Task10.Data;
using Task10.Models.Base;
using Task10.Repository.Interfaces;

namespace Task10.Repository.Base
{
    internal class RepositoryBase<T> : IRepository<T> where T : Entity, new()
    {
        private readonly Task10DbContext _db;
        private readonly DbSet<T> _dbSet;

        public RepositoryBase(Task10DbContext db)
        {
            _db = db;
            _dbSet = _db.Set<T>();
        }

        public virtual IQueryable<T> Items => _dbSet;

        public virtual T Get(int id)
        {
            T? item = Items.SingleOrDefault(item => item.Id == id);

            return item;
        }

        public virtual async Task<T> GetAsync(int id, CancellationToken cancel = default)
        {
            T? item = await Items
                        .SingleOrDefaultAsync(item => item.Id == id, cancel)
                        .ConfigureAwait(false);
            
            return item;
        }

        public T Add(T item)
        {
            if (item is null)
                throw new ArgumentNullException($"Exeption! Lost data. The object {nameof(item)} is null.");

            _db.Entry(item).State = EntityState.Added;
            _db.SaveChanges();

            return item;
        }

        public async Task<T> AddAsync(T item, CancellationToken cancel = default)
        {
            if (item is null)
                throw new ArgumentNullException($"Exeption! Lost data. The object {nameof(item)} is null.");

            _db.Entry(item).State = EntityState.Added;
            await _db.SaveChangesAsync(cancel).ConfigureAwait(false);

            return item;
        }

        public void Update(T item)
        {
            if (item is null)
                throw new ArgumentNullException($"Exeption! Lost data. The object {nameof(item)} is null.");

            _db.Entry(item).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public async Task UpdateAsync(T item, CancellationToken cancel = default)
        {
            if (item is null)
                throw new ArgumentNullException($"Exeption! Lost data. The object {nameof(item)} is null.");

            _db.Entry(item).State = EntityState.Modified;
            await _db.SaveChangesAsync(cancel).ConfigureAwait(false);
        }

        public void Remove(int id)
        {
            var item = Get(id);
            if (item is null)
                throw new InvalidOperationException($"Exeption! The {item} with ID={id} was not found in the context.");

            _db.Remove(item);
            _db.SaveChanges();
        }

        public async Task RemoveAsync(int id, CancellationToken cancel = default)
        {
            var item = await GetAsync(id, cancel);
            if (item is null)
                throw new InvalidOperationException($"Exeption! The {item} with ID={id} was not found in the context.");

            _db.Remove(item);
            await _db.SaveChangesAsync(cancel).ConfigureAwait(false);
        }

        public void AddRange(IEnumerable<T> items)
        {
            if (items is null)
                throw new ArgumentNullException($"Exception! Lost data. The {nameof(items)} collection is null.");

            _dbSet.AddRange(items);
            _db.SaveChanges();
        }

        public async Task AddRangeAsync(IEnumerable<T> items, CancellationToken cancel = default)
        {
            if (items is null)
                throw new ArgumentNullException($"Exception! Lost data. The {nameof(items)} collection is null.");

            await _dbSet.AddRangeAsync(items, cancel).ConfigureAwait(false);
            await _db.SaveChangesAsync(cancel).ConfigureAwait(false);
        }

        public void UpdateRange(IEnumerable<T> items)
        {
            if (items is null)
                throw new ArgumentNullException($"Exception! Lost data. The {nameof(items)} collection is null.");

            _dbSet.UpdateRange(items);
            _db.SaveChanges();
        }

        public async Task UpdateRangeAsync(IEnumerable<T> items, CancellationToken cancel = default)
        {
            if (items is null)
                throw new ArgumentNullException($"Exception! Lost data. The {nameof(items)} collection is null.");

            foreach (T item in items)
                _db.Entry(item).State = EntityState.Modified;
            await _db.SaveChangesAsync(cancel).ConfigureAwait(false);
        }
    }
}
