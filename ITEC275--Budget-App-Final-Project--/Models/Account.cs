using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ITEC275__Budget_App_Final_Project__.Models
{
    public class Account
    {
        public int AccountId { get; set; }

        public decimal TotalAssets { get; set; }

        public string AccountName { get; set; }
    }
}
