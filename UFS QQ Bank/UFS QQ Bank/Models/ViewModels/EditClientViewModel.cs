using System.ComponentModel.DataAnnotations;

namespace UFS_QQ_Bank.Models.ViewModels
{
    public class EditClientViewModel
    {
        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Gender { get; set; }

        [DataType(DataType.Date)]
        public string DateOfBirth { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [RegularExpression(@"^\d{13}$", ErrorMessage = "Invalid ID number")]
        public string IDPassportNumber { get; set; }

        public string UserType { get; set; }

        public string StaffStudentNumber { get; set; }

       
        public string Password { get; set; }
    }
}
