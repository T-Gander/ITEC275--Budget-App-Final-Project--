using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITEC275__Budget_App_Final_Project__.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        public int SectionId { get; set; }

        public decimal AssignedBudget { get; set; }

        public string? CategoryName { get; set; }

        [ForeignKey("SectionId")]
        public Section? Section { get; set; }
    }
}
