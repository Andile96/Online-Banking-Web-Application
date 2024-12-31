using UFS_QQ_Bank.Models;

namespace UFS_QQ_Bank.Data
{
    public interface IAdviceRepository: IRepositoryBase<Advice>
    {
        Task AddAdviceAsync(Advice advice);
        Task<IEnumerable<Advice>> GetAllAdvicesAsync();
        Task<IEnumerable<Advice>> GetAdvicesByAccountNumberAsync(string accountNumber);
    }
}
