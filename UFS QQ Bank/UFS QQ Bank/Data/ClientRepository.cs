using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UFS_QQ_Bank.Models;
using UFS_QQ_Bank.Models.ViewModels;

namespace UFS_QQ_Bank.Data
{
    public class ClientRepository : RepositoryBase<User>, IClientRepository
    {

        
        private readonly UserManager<User> _userManager;

        public ClientRepository(AppEntityDbContext db, UserManager<User> userManager) : base(db)
        {
            
            _userManager = userManager;
        }

        public async Task<List<ClientUserViewModel>> GetAllUsersDetails()
        {
            return await (from user in _userManager.Users
                          where user.UserType == "Staff" || user.UserType == "Student"
                          select new ClientUserViewModel
                          {
                              User = user,
                              
                             
                          
                          }).ToListAsync();
           

        }
      


    }
}
