using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossPlatform.Infrastructure
{
    public abstract class SqliteRepository<TEntity> where TEntity : new()
    {
        /// <summary>
        /// Instance
        /// </summary>
        public static SqliteRepository<TEntity> Instance { get; set; }

        /// <summary>
        /// Add item
        /// </summary>
        /// <param name="newItem"></param>
        /// <returns></returns>
        public abstract Task<int> AddAsync(TEntity newItem);

        public abstract Task<int> AddAsync(IEnumerable<TEntity> newItems);

        /// <summary>
        /// Load item
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public abstract IQueryable<TEntity> Load(
            System.Linq.Expressions.Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? page = null,
            int? pageSize = null);

        /// <summary>
        /// Remove Item
        /// </summary>
        /// <param name="removeItem"></param>
        /// <returns></returns>
        public abstract Task<int> RemoveAsync(TEntity removeItem);

        /// <summary>
        /// Update Item
        /// </summary>
        /// <param name="editItem"></param>
        /// <returns></returns>
        public abstract Task<int> UpdateAsync(TEntity editItem);

        /// <summary>
        /// Clear Table
        /// </summary>
        /// <returns></returns>
        public abstract int ClearTable();
    }
}
