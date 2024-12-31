namespace UFS_QQ_Bank.Models.ViewModels
{
    public class PagingInfoViewModel
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages =>
           (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);

        public List<User> Users { get; set; } = new List<User>();
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
