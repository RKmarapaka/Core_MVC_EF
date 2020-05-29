using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core_MVC_EF.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50, ErrorMessage = "Name cannot exeed 50 character") ]
        public string Name { get; set; }

        [Required, Display(Name = "Office Email")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$",
                            ErrorMessage = "Invalid Email Format")]
        public string Email { get; set; }

        [Required]
        public string City { get; set; }
        [Required]
        public int DepartmentId { get; set; }
        
        public string PhotoPath { get; set; }
    }
}
