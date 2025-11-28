using Demo.DAL.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.PL.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is Required")]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        [MinLength(3, ErrorMessage = "Name must be at least 3 characters long")]
        public string Name { get; set; }
        [ValidateNever]
        public IFormFile Image { get; set; }
        [ValidateNever]
        public string ImageName { get; set; }
        [Range(18, 65, ErrorMessage = "Age must be between 18 and 65")]
        public int? Age { get; set; }
        [RegularExpression(@"[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$",
        ErrorMessage = "Address must be like 123-Street-City-Country")]
        public string Address { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Phone number must be 11 digits")]
        public string Phone { get; set; }

        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        [DisplayName("Hiring Date")]
        public DateOnly HireDate { get; set; }
       
        #region Relations

        [ForeignKey("Department")]
        // Nullable Foreign Key => On delete : restrict
        // Non-Nullable Foreign Key => On delete : Cascade
        public int? DepartmentId { get; set; } // Foreign Key
        [InverseProperty("Employees")]
        [ValidateNever]
        public Department Department { get; set; } // Navigation Property for one

        #endregion
    }
}
