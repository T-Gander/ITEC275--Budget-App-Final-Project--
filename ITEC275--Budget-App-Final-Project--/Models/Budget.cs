using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITEC275__Budget_App_Final_Project__.Models
{
    public class Budget
    {
        public int BudgetId { get; set; }

        [ForeignKey("User")]
        public string OwnerId { get; set; }

        public string BudgetName { get; set; }  

        public User User { get; set; }

    }
}
