using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models
{
    public class Department
    {
        public int Id { get; set; }// is always required
        // required make it not-nullable and the errorMessage is only in front not stored in the database
        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Code is Required")]
        public string Code { get; set; }// reference data types are nullable by default
        public DateOnly DateOfCreation { get; set; }

        // Navigation Property
        [InverseProperty("Department")]
        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();

    }
}
