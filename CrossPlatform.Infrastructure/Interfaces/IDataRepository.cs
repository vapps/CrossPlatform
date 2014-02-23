using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossPlatform.Infrastructure.Interfaces
{
    public interface IDataRepository<TEntity> where TEntity : new()
    {
        Task Add(TEntity newItem);
        Task<IQueryable<TEntity>> Load();
        Task Remove(TEntity removeItem);
        Task Update(TEntity editItem);
    }

}
