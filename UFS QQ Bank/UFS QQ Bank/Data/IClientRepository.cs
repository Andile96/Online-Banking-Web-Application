using UFS_QQ_Bank.Models;
using UFS_QQ_Bank.Models.ViewModels;

namespace UFS_QQ_Bank.Data
{
    public interface IClientRepository : IRepositoryBase<User>
    {
        Task<List<ClientUserViewModel>> GetAllUsersDetails();
     
    }
}
