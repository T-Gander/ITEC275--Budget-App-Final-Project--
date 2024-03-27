using ITEC275__Budget_App_Final_Project__.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ITEC275__Budget_App_Final_Project__.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Account> Accounts { get; set; }

        public DbSet<Budget> Budgets { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Connection> Connections { get; set; }

        public DbSet<Section> Sections { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Account>().HasKey(x => x.AccountId);

            builder.Entity<Budget>().HasKey(x => x.BudgetId);

            builder.Entity<Category>().HasKey(x => x.CategoryId);

            builder.Entity<Connection>().HasKey(x => x.TransactionId);

            builder.Entity<Section>().HasKey(x => x.SectionId);

            builder.Entity<Transaction>().HasKey(x => x.TransactionId);
        }
    }
}
