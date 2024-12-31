using Microsoft.EntityFrameworkCore;
using UFS_QQ_Bank.Models;

namespace UFS_QQ_Bank.Data
{
    public class AdviceRepository : RepositoryBase<Advice>, IAdviceRepository
    {
        public AdviceRepository(AppEntityDbContext db) : base(db)
        {
        }

        public async Task AddAdviceAsync(Advice advice)
        {
            await _db.Advices.AddAsync(advice);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Advice>> GetAllAdvicesAsync()
        {
            return await _db.Advices.ToListAsync();
        }

        public async Task<IEnumerable<Advice>> GetAdvicesByAccountNumberAsync(string accountNumber)
        {
            return await _db.Advices
                .Where(a => a.AccountNumber == accountNumber)
                .ToListAsync();
        }
    }
}
