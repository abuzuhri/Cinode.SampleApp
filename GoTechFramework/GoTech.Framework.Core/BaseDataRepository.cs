using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GoTech.Framework.Core
{
    public class BaseDataRepository<T> where T : BaseGoTechEntity
    {
        private readonly BaseGoTechContext _dbContext;
        private readonly DbSet<T> _dbSet;
        string errorMessage = string.Empty;

        public BaseDataRepository(BaseGoTechContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dbSet = _dbContext.Set<T>();
        }
        public T GetById(object id) => _dbSet.Find(id);
        public async Task<T> FindAsync(params object[] keyValues) => await _dbSet.FindAsync(keyValues);
        public IQueryable<T> Query(Expression<Func<T, bool>> predicate) => _dbSet.AsNoTracking().Where(predicate);
        public IQueryable<T> Where(Expression<Func<T, bool>> predicate) => _dbSet.Where(predicate);
        public IQueryable<T> FromSql(string sql, params object[] parameters) => _dbSet.FromSql(sql, parameters);
        public void Insert(T entity) => _dbSet.Add(entity);
        public async Task InsertAsync(T entity) => await _dbSet.AddAsync(entity);
        public async Task InsertAsync(params T[] entities) => await _dbSet.AddRangeAsync(entities);
        public async Task InsertAsync(IEnumerable<T> entities) => await _dbSet.AddRangeAsync(entities);
        public void Update(T entity) => _dbSet.Update(entity);
        public void Update(params T[] entities) => _dbSet.UpdateRange(entities);
        public void Update(IEnumerable<T> entities) => _dbSet.UpdateRange(entities);


        public void Delete(object id)
        {
            var entity = GetById(id);
            Delete(entity);
        }

        public void Delete(T entity) => _dbSet.Remove(entity);

        public void Delete(params T[] entities) => _dbSet.RemoveRange(entities);
        public void Delete(IEnumerable<T> entities) => _dbSet.RemoveRange(entities);

        public IQueryable<T> Table
        {
            get
            {
                return _dbSet;
            }
        }

        //private DbSet<T> Entities
        //{
        //    get
        //    {
        //        if (_dbSet == null)
        //        {
        //            _dbSet = _dbContext.Set<T>();
        //        }
        //        return _dbSet;
        //    }
        //}
    }
}
