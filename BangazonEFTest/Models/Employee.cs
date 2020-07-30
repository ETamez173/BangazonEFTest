using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonEFTest.Models
{
    public class Employee
    {
        [Required]
        public int Id { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is required!")]
        [MinLength(2, ErrorMessage = "First Name should be at least 2 characters long")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name is required!")]
        [MinLength(3, ErrorMessage = "Last Name should be at least 3 characters long")]
        public string LastName { get; set; }

        [Display(Name = "Email")]
        [Required]
        public string Email { get; set; }

        [Display(Name = "Department")]
        [Required]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        [Display(Name = "Assigned Computer")]
        [Required]
        public int ComputerId { get; set; }
        public Computer Computer { get; set; }


    }
}
