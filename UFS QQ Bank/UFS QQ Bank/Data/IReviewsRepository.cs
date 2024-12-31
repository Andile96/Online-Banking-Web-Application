using UFS_QQ_Bank.Models;

namespace UFS_QQ_Bank.Data
{
    public interface IReviewsRepository: IRepositoryBase<Reviews>
    {
        Task AddReview(Reviews review);
        Task<IEnumerable<Reviews>> GetAllReviews();
    }
}
