using System.ComponentModel.DataAnnotations;

namespace UFS_QQ_Bank.Models.ViewModels
{
    public class LoginModel
    {
        [Required]
        [UIHint("email")]
        public string Email { get; set; }

        [Required]
        [UIHint("password")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; } = "/";
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
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
        [DataType(DataType.Password)] 
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords must match")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
