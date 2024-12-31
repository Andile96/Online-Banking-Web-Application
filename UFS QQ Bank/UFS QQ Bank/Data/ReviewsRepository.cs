using Microsoft.EntityFrameworkCore;
using UFS_QQ_Bank.Models;

namespace UFS_QQ_Bank.Data
{
    public class ReviewsRepository : RepositoryBase<Reviews>, IReviewsRepository
    {
        public ReviewsRepository(AppEntityDbContext db) : base(db)
        {
        }

        public async Task AddReview(Reviews review)
        {
            review.CreatedAt = DateTime.Now;
            _db.Reviews.Add(review);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Reviews>> GetAllReviews()
        {
            return await _db.Reviews.OrderByDescending(r => r.CreatedAt).ToListAsync();
        }
    }
}
