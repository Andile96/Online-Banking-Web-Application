namespace UFS_QQ_Bank.Data
{
    public interface IRepositoryWrapper
    {
        IClientRepository client {  get; }
        IBankAccountRepository bankAccount { get; }
        INotificationRepository Notification { get; }
        ITransactionRepository Transaction {  get; }
        IReviewsRepository Reviews { get; }
        IAdviceRepository Advices { get; }
        IReportRepository Reports { get; }
        IClientActivityRepository ClientActivity { get; }

        Task SaveChanges();
    }
}
