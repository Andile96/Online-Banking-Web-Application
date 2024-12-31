using System.ComponentModel.DataAnnotations;

namespace UFS_QQ_Bank.Models.ViewModels
{
    public class DeleteClientViewModel
    {
       

        public string FirstName { get; set; }

        public string UserName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }

        public string IDOrPassportNumber { get; set; }

        public string Phone { get; set; }
        public string EmployeeOrStudentNumber { get; set; }

        public string UserType { get; set; }
    }
}
