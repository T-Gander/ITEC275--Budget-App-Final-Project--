using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITEC275__Budget_App_Final_Project__.Models
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; }

        public int BudgetId { get; set; }

        public string AccountName { get; set; }

        [ForeignKey(nameof(BudgetId))]
        public Budget Budget { get; set; }

    }
}
