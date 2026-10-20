using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.PlanViewModels
{
    public class UpdatePlanViewModel
    {
        public string PlanName { get; set; } = null!;
        [Required(ErrorMessage = "Description Is Required")]
        [StringLength(maximumLength: 200, MinimumLength = 5, ErrorMessage = "Description Must be Between 5 and 200 Char")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Duration Days Is Required")]
        [Range(minimum: 1, maximum: 365, ErrorMessage = "Duration Days Must Be Between 1 and 365")]
        public int DurationDays { get; set; }

        [Required(ErrorMessage = "Price Is Required")]
        [Range(minimum: 0.1, maximum: 10000, ErrorMessage = "Price Must Be Between [ and 365")]
        public decimal Price { get; set; }
    }
}
