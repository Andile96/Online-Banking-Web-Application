using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UFS_QQ_Bank.Models;

namespace UFS_QQ_Bank.Data
{
    public class AppEntityDbContext: IdentityDbContext<User>
    {

        public AppEntityDbContext(DbContextOptions options) : base(options)
        { 
 
        }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Reviews> Reviews { get; set; }
        public DbSet<Advice> Advices { get; set; }
        public DbSet<Reports> Reports { get; set; }
        public DbSet<ClientActivity> ClientActivities { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BankAccount>().ToTable("BankAccounts");
            modelBuilder.Entity<Notification>().ToTable("Notifications");
            modelBuilder.Entity<Transaction>().ToTable("Transactions");
            modelBuilder.Entity<Reviews>().ToTable("Reviews");
            modelBuilder.Entity<Advice>().ToTable("Advices");
            modelBuilder.Entity<Reports>().ToTable("Reports");
            modelBuilder.Entity<ClientActivity>().ToTable("ClientActivity");


        }
    }
}
