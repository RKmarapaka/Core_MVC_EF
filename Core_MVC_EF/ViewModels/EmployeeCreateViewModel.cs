using Core_MVC_EF.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core_MVC_EF.ViewModels
{
    public class EmployeeCreateViewModel
    {
        [Required, MaxLength(50, ErrorMessage = "Name cannot exeed 50 character")]
        public string Name { get; set; }

        [Required, Display(Name = "Office Email")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$",
                            ErrorMessage = "Invalid Email Format")]
        public string Email { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        [Display(Name = "Department")]
        public int DepartmentId { set; get; }
        public Department Department { get; set; }

        public IFormFile Photo { get; set; }
    }
}
