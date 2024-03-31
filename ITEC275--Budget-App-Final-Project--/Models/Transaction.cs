using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITEC275__Budget_App_Final_Project__.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }

        public int AccountId { get; set; }

        public int? CategoryId { get; set; }

        public string Payee { get; set; }

        public DateTime TransactionDate { get; set; }

        public decimal TransactionValue { get; set; }

        public bool IsCredit { get; set; }

        [ForeignKey(nameof(AccountId))]
        public Account Account { get; set; }
    }
}
