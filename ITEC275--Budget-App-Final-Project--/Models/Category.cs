using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ITEC275__Budget_App_Final_Project__.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }    
    }
}
