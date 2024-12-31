namespace UFS_QQ_Bank.Models.ViewModels
{
    public class ClientListViewModel
    {
        public IEnumerable<User> Users { get; set; }
        public PagingInfoViewModel PagingInfo { get; set; }
    }
}
