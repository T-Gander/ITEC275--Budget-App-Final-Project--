using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITEC275__Budget_App_Final_Project__.Models
{
    public class Connection
    {
        //This enforces the one to one relationship with Transactions Table
        [Key, ForeignKey("Transaction")]
        public int TransactionId { get; set; }

        public Transaction Transaction { get; set; }

        [ForeignKey("Budget")]
        public int BudgetId { get; set; }

        public Budget Budget { get; set; }

        [ForeignKey("Section")]
        public int SectionId { get; set; }

        public Section Section { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        [ForeignKey("Account")]
        public int AccountId { get; set; }

        public Account Account { get; set; }    
    }
}
