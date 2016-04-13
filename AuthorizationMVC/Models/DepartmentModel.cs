using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AuthorizationMVC.Models
{
    public class DepartmentModel
    {
        [Required]
        [Display(Name = "Department name")]
        public string Name { get; set; }
    }

    public class PositionModel
    {
        [Required]
        [Display(Name = "Position name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Department")]
        public string IdDepartment { get; set; }
    }
}