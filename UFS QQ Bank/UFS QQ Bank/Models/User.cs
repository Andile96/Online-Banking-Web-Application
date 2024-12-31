using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace UFS_QQ_Bank.Models
{
    public class User: IdentityUser
    {
     

        public byte[] Profile { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }
        public string DateOfBirth { get; set; }

        public string IDOrPassportNumber { get; set; }

        public string Phone { get; set; }
        public string EmployeeOrStudentNumber { get; set; }

        public string UserType { get; set; } // Staff or Student 0r external user
        public string MobilePassword { get; set; }


       
       

    }
}
