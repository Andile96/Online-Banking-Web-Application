using Microsoft.AspNetCore.Identity;
using UFS_QQ_Bank.Data.DataAccess;
using UFS_QQ_Bank.Models;

namespace UFS_QQ_Bank.Data
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly AppEntityDbContext _db;
        private readonly UserManager<User> _user;

        private IClientRepository _client;
        private IBankAccountRepository _bankAccount;
        private INotificationRepository _notification;
        private IReviewsRepository _Reviews;
        private IAdviceRepository _Advice;
        private IReportRepository _Report;
        private IClientActivityRepository _ClientActivity;
        private ITransactionRepository _transaction;
        public RepositoryWrapper(AppEntityDbContext db, UserManager<User> user)
        {
            _db = db;
            _user = user;

        }
        public IClientRepository client
        {
            get 
            { 
                if ( _client == null )
                {
                    _client = new ClientRepository(_db,_user);
                }
                return _client;
            }
        }

        public IBankAccountRepository bankAccount
        {
            get
            {
                if(_bankAccount == null)
                {
                    _bankAccount = new BankAccountRepository(_db);
                }
                return _bankAccount;
            }
        }

        public INotificationRepository Notification
        {
            get
            {
                if (_notification == null)
                {
                    _notification = new NotificationRepository(_db);
                }
                return _notification;
            }
        }

        public IReviewsRepository Reviews
        {
            get
            {
                if(_Reviews==null)
                {
                    _Reviews = new ReviewsRepository(_db);
                }
                return _Reviews;
            }
        }

        public IAdviceRepository Advices
        {
            get
            {
                if(_Advice ==null)
                {
                    _Advice = new AdviceRepository(_db);
                }
                return _Advice;
            }
        }

        public IReportRepository Reports
        {
            get
            {
                if(_Report ==null)
                {
                    _Report = new ReportRepository(_db);
                }
                return _Report;
            }
        }

        public IClientActivityRepository ClientActivity
        {
            get
            {
                if(_ClientActivity == null)
                {
                    _ClientActivity = new ClientActivityRepository(_db);
                }
                return _ClientActivity;
            }
        }

        public ITransactionRepository Transaction
        {
            get
            {
                if(_transaction == null)
                {
                    _transaction = new TransactionRepository(_db);
                }

                return _transaction;
            }
        }

        public async Task SaveChanges()
        {
            await _db.SaveChangesAsync();
        }
    }
}
