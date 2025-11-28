using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Demo.DAL.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public string ImageName { get; set; }
        public int? Age { get; set; }
        
        public string Address { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        [Precision(18, 2)]
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        public DateOnly HireDate { get; set; }

        [DisplayName("Date of Creation")]
        public DateOnly DateOfCreation { get; set; } = DateOnly.FromDateTime(DateTime.Now);


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
