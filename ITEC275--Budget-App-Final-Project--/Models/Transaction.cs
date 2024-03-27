using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ITEC275__Budget_App_Final_Project__.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }

        public DateTime TransactionDate { get; set; }

        public decimal TransactionValue { get; set; }

        public bool IsCredit { get; set; }
    }
}
