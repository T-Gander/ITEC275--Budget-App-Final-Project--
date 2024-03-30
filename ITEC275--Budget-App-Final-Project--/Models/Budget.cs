using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITEC275__Budget_App_Final_Project__.Models
{
    public class Budget
    {
        [Key]
        public int BudgetId { get; set; }
        
        public string UserId { get; set; }

        public string BudgetName { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

    }
}
