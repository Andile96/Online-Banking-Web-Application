using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UFS_QQ_Bank.Data.DataAccess;
using UFS_QQ_Bank.Models;

namespace UFS_QQ_Bank.Data
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected AppEntityDbContext _db;
      
        public RepositoryBase(AppEntityDbContext db)
        {
            _db = db;
            
        }
        //public void Create(T entity)
        //{
        //    _db.Set<T>().Add(entity);
        //}

        //public void Delete(T entity)
        //{
        //    _db.Set<T>().Remove(entity);
        //}

        //public IEnumerable<T> FindAll()
        //{
        //    return _db.Set<T>();
        //}

        //public IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression)
        //{
        //    return _db.Set<T>().Where(expression);
        //}

        //public T GetById(int id)
        //{
        //    return _db.Set<T>().Find(id);
        //}

        //public IEnumerable<T> GetWithOptions(QueryOptions<T> options)
        //{
        //    IQueryable<T> query = _db.Set<T>();

        //    if (options.HasWhere)
        //        query = query.Where(options.Where);

        //    if (options.HasOrderBy)
        //    {
        //        if (options.OrderByDirection == "asc")
        //            query = query.OrderBy(options.OrderBy);
        //        else
        //            query = query.OrderByDescending(options.OrderBy);
        //    }
        //    if (options.HasPaging)
        //    {
        //        query = query.Skip((options.PageNumber - 1) * options.PageSize)
        //                     .Take(options.PageSize);
        //    }

        //    return query.ToList();
        //}

        //public void Update(T entity)
        //{
        //    _db.Set<T>().Update(entity);
        //}

        public int GetCount(QueryOptions<T> options)
        {
            IQueryable<T> query = _db.Set<T>();

            
            if (options.HasWhere)
            {
                query = query.Where(options.Where);
            }

         
            return query.Count();
        }

        public async Task<T> AddAsync(T client)
        {
            await _db.AddAsync(client);
            return client;
        }


        public IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return _db.Set<T>().Where(expression);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _db.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _db.Set<T>().FindAsync(id);
        }

        public IEnumerable<T> GetWithOptions(QueryOptions<T> options)
        {
            IQueryable<T> query = _db.Set<T>();

            if (options.HasWhere)
                query = query.Where(options.Where);

            if (options.HasOrderBy)
            {
                if (options.OrderByDirection == "asc")
                    query = query.OrderBy(options.OrderBy);
                else
                    query = query.OrderByDescending(options.OrderBy);
            }

            if (options.HasPaging)
            {
                query = query.Skip((options.PageNumber - 1) * options.PageSize)
                             .Take(options.PageSize);
            }

            return query.ToList();
        }

        public async Task RemoveAsync(int id)
        {
            var getbyId = await GetByIdAsync(id);
            if (getbyId != null)
            {
                _db.Set<T>().Remove(getbyId);
                await _db.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(T client)
        {
            _db.Set<T>().Update(client);
            await _db.SaveChangesAsync();
        }


    }
}
