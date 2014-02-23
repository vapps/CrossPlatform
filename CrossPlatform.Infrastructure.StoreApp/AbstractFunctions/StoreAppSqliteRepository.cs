using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Microsoft.Practices.Prism.PubSubEvents;

namespace CrossPlatform.Infrastructure.StoreApp
{
    public class StoreAppSqliteRepository<TEntity> : SqliteRepository<TEntity> where TEntity : new()
    {
        //internal ConcurrentBag<TEntity> _context;
        IEventAggregator _eventAggregator;

        internal string _dbPath;

        //internal TableMapping _tableMap;

        public StoreAppSqliteRepository(IEventAggregator eventAggregator, string dbPath)
        {
            _eventAggregator = eventAggregator;
            _dbPath = dbPath;
            //_context = new ConcurrentBag<TEntity>();

            Initialize();
        }

        private void Initialize()
        {
            using (var db = new SQLiteConnection(_dbPath))
            {
                db.CreateTable<TEntity>();
                var result = db.Table<TEntity>().Count();
                if(result == 0)
                {
                    StaticFunctions.InvokeIfRequiredAsync(StaticFunctions.BaseContext,
                    para =>
                    {
                        _eventAggregator.GetEvent<CrossPlatform.Infrastructure.Events.SqliteCreateTableEvent>().Publish(new KeyValuePair<string, object>(typeof(TEntity).Name, true));
                    }, null);
                }

                db.Commit();
                db.Close();
                db.Dispose();
            }
        }

        public override int ClearTable()
        {
            int returnValue;
            using (var db = new SQLiteConnection(_dbPath))
            {
                returnValue =  db.DropTable<TEntity>();
                if (returnValue == 0)
                {
                    db.CreateTable<TEntity>();
                    db.Commit();
                    db.Close();
                    db.Dispose();
                }
            }
            return returnValue;
        }

        public override async Task<int> AddAsync(TEntity newItem)
        {
            var conn = new SQLiteAsyncConnection(_dbPath);
            var result = await conn.InsertAsync(newItem);
            return result;
        }

        public override async Task<int> AddAsync(IEnumerable<TEntity> newItems)
        {
            var conn = new SQLiteAsyncConnection(_dbPath);
            var result = await conn.InsertAllAsync(newItems);
            return result;
        }
 
        public override async Task<int> RemoveAsync(TEntity removeItem)
        {
            var conn = new SQLiteAsyncConnection(_dbPath);
            var result = await conn.DeleteAsync(removeItem);
            return result;
        }

        public override async Task<int> UpdateAsync(TEntity editItem)
        {
            var conn = new SQLiteAsyncConnection(_dbPath);
            var result = await conn.UpdateAsync(editItem);
            return result;
        }

        public override IQueryable<TEntity> Load(
            System.Linq.Expressions.Expression<Func<TEntity, bool>> filter = null, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, 
            int? page = null, 
            int? pageSize = null)
        {
            var conn = new SQLiteConnection(_dbPath);

            IQueryable<TEntity> query = conn.Table<TEntity>().AsQueryable();

            //조건절이 있으면 추가
            if (filter != null)
            {
                query = query.Where(filter);
            }

            //정렬값이 있으면 정렬하고 아니면 그냥 보내고..
            if (orderBy != null)
            {
                orderBy(query).AsQueryable();
            }

            //페이지 처리
            if (page != null && pageSize != null)
            {
                int pageNo = Convert.ToInt32(page) - 1;
                int pSize = Convert.ToInt32(pageSize);

                if (pageSize <= 0)
                {
                    pageSize = 10; // default
                }

                if (pageNo <= 0)
                {
                    pageNo = 0;
                }

                int skipSize = pageNo * pSize;
                return query.Skip(skipSize).Take(pSize).AsQueryable();
            }
            else
            {
                return query.AsQueryable();
            }
        }
    }
}
