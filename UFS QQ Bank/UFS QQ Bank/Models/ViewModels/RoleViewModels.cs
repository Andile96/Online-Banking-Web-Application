using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace UFS_QQ_Bank.Models.ViewModels
{
    public class RoleEditModel
    {
        public IdentityRole Role { get; set; }
        public IEnumerable<User> InternalClient { get; set; }
        public IEnumerable<User> ExternalClient { get; set; }
    }

    public class RoleModificationModel
    {
        [Required]
        public string RoleName { get; set; }
        public string RoleId { get; set; }
        public string[] IdsToAdd { get; set; }
        public string[] IdsToDelete { get; set; }
    }
}
