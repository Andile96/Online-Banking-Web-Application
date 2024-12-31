using System.Linq.Expressions;
using UFS_QQ_Bank.Data.DataAccess;

namespace UFS_QQ_Bank.Data
{
    public interface IRepositoryBase<T> where T : class
    {
      
        IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression);
        IEnumerable<T> GetWithOptions(QueryOptions<T> options);
        int GetCount(QueryOptions<T> options);
        Task<T> AddAsync(T client);
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task RemoveAsync(int id);
        Task UpdateAsync(T client);

    }
}
