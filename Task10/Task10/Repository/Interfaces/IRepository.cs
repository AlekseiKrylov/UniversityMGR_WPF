using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Task10.Models.Base.Interfaces;

namespace Task10.Repository.Interfaces
{
    internal interface IRepository<T> where T : class, IEntity, new()
    {
        IQueryable<T> Items { get; }

        T Get(int id);

        Task<T> GetAsync(int id, CancellationToken cancel = default);

        T Add(T item);

        Task<T> AddAsync(T item, CancellationToken cancel = default);

        void Update(T item);

        Task UpdateAsync(T item, CancellationToken cancel = default);

        void Remove(int id);

        Task RemoveAsync(int id, CancellationToken cancel = default);

        void AddRange(IEnumerable<T> items);
        
        Task AddRangeAsync(IEnumerable<T> items, CancellationToken cancel = default);

        void UpdateRange(IEnumerable<T> items);

        Task UpdateRangeAsync(IEnumerable<T> items, CancellationToken cancel = default);
    }
}
