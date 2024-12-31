using System.ComponentModel.DataAnnotations;

namespace UFS_QQ_Bank.Models.ViewModels
{
    public class CreateEmployeeUserViewModel
    {
        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Role { get; set; } // Consultant, FinancialAdvisor or Client

        [Required]
        public string StaffStudentNumber { get; set; }
        [Required]
        public string Password { get; set; }
    }
    public class CreateClientUserModel
    {
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public string DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Mobile No")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid mobile number")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "E-mail")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "RSA ID or Passport number ")]
        [UIHint("Enter you 13 digit RSA ID or Passport number for non SA citizen")]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "Invalid ID number")]
        public string IDPassportNumber { get; set; }

        [Required]
        [Display(Name = "User type")]
        [UIHint("Indicate if you a Student, Staff or external user")]
        public string UserType { get; set; }

        [Required]
        [Display(Name = "staff or student number")]
        public string StaffStudentNumber { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
