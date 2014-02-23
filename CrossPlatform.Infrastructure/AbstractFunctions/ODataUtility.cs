using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrossPlatform.Infrastructure.AbstractFunctions
{
    public abstract class ODataUtility<TEntity> where TEntity : new()
    {
        /// <summary>
        /// Instance
        /// </summary>
        public static ODataUtility<TEntity> Instance { get; set; }

        /// <summary>
        /// Add item
        /// </summary>
        /// <param name="newItem"></param>
        /// <returns></returns>
        public abstract System.Threading.Tasks.Task<int> AddAsync(TEntity newItem);

        public abstract System.Threading.Tasks.Task<int> AddAsync(IEnumerable<TEntity> newItems);

        /// <summary>
        /// Load item
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public abstract IQueryable<TEntity> Get(
            System.Linq.Expressions.Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? page = null,
            int? pageSize = null);

        /// <summary>
        /// Remove Item
        /// </summary>
        /// <param name="removeItem"></param>
        /// <returns></returns>
        public abstract System.Threading.Tasks.Task<int> RemoveAsync(TEntity removeItem);

        /// <summary>
        /// Update Item
        /// </summary>
        /// <param name="editItem"></param>
        /// <returns></returns>
        public abstract System.Threading.Tasks.Task<int> UpdateAsync(TEntity editItem);

    }
}
