namespace UFS_QQ_Bank.Models.ViewModels
{
    public class AppViewModel
    {
        public List<ClientUserViewModel> ClientUsers { get; set; }
        public PagingInfoViewModel PagingInfo { get; set; }
        public string SearchString { get; set; }
        public string SortOrder { get; set; }
    }
}
