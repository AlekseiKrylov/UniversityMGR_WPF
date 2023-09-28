using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UniversityMGR_WPF.Models.Base.Interfaces;

namespace UniversityMGR_WPF.Services.DbServices.Interfaces
{
    internal interface IDbService<T> where T : class, IEntity, new()
    {
        IQueryable<T> Items { get; }

        Task<T> GetDetailAsync(int id, CancellationToken cancel = default);

        Task<T> AddAsync(T item, CancellationToken cancel = default);

        Task UpdateAsync(T item, CancellationToken cancel = default);

        Task RemoveAsync(int id, CancellationToken cancel = default);

        Task AddRangeAsync(IEnumerable<T> items, CancellationToken cancel = default);

        Task UpdateRangeAsync(IEnumerable<T> items, CancellationToken cancel = default);
    }
}
