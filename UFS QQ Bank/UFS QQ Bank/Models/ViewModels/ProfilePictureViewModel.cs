namespace UFS_QQ_Bank.Models.ViewModels
{
    public class ProfilePictureViewModel
    {
        public byte[] ProfilePicture { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

      
        public string Initials
        {
            get
            {
                return $"{FirstName?[0]}{LastName?[0]}".ToUpper();
            }
        }
    }
}
