using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITEC275__Budget_App_Final_Project__.Models
{
    public class Section
    {
        [Key]
        public int SectionId { get; set; }

        public int BudgetId { get; set; }

        public string SectionName { get; set; }

        [ForeignKey("BudgetId")]
        public Budget Budget { get; set; }

    }
}
