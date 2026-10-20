using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MemberViewModels
{
    public class MemberToUpdateViewModel
    {
        public string Name { get; set; } = null!;
        public string? Photo { get; set; }


        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress(ErrorMessage = "Invalid email Format")] // Validation
        [DataType(dataType: DataType.EmailAddress)] // UI HINT
        [StringLength(maximumLength: 100, MinimumLength = 5, ErrorMessage = "Email Must Be Between 5 and 100 Char")]
        public string Email { get; set; } = null!;


        [Required(ErrorMessage = "Phone Is Required")]
        [Phone(ErrorMessage = "Invalid Phone Format")] // Validation
        [RegularExpression(pattern: @"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone Number Must Be Valid Egyptian PhoneNumber")]
        [DataType(DataType.PhoneNumber)] // UI HINT
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "Building Number Is Required")]
        [Range(minimum: 1, maximum: 9000, ErrorMessage = "Building Number Must Be Between 1 and 9000")]
        public int BuildingNumber { get; set; }


        [Required(ErrorMessage = "Street Is Required")]
        [StringLength(maximumLength: 30, MinimumLength = 2, ErrorMessage = "Street Must Be Between 2 and 30 Chars")]
        public string Street { get; set; } = null!;


        [Required(ErrorMessage = "City Is Required")]
        [StringLength(maximumLength: 30, MinimumLength = 2, ErrorMessage = "City Must Be Between 2 and 30 Chars")]
        [RegularExpression(pattern: @"^[a-zA-Z\s]+$", ErrorMessage = "City Can Contain Only Letters And Spaces")]
        public string City { get; set; } = null!;


    }
}
