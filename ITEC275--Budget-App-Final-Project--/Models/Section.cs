using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ITEC275__Budget_App_Final_Project__.Models
{
    public class Section
    {
        [Key]
        public int SectionId { get; set; }

        public string SectionName { get; set; } 
    }
}
