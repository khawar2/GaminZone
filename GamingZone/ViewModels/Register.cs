using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GamingZone.ViewModels
{
    public class Register
    {
        [Display(Name = "First Name:")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "First Name is Required.")]
        [StringLength(50, MinimumLength = 3,
       ErrorMessage = "First Name should be minimum 3 characters and a maximum of 50 characters")]
        public string firstname { get; set; }


        [Display(Name = "Last Name:")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Last Name is Required.")]
        [StringLength(50, MinimumLength = 3,
        ErrorMessage = "Last Name should be minimum 3 characters and a maximum of 50 characters")]
        public string lastname { get; set; }


        [Display(Name = "Email:")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email is Required.")]
        public string email { get; set; }
        

        [Display(Name = "Create Password:")]
        [Required(ErrorMessage = "Password is Required.")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 8,
        ErrorMessage = "Password should be minimum 8 characters and a maximum of 20 characters")]
        public string Password { get; set; }

        [Display(Name = "Role:")]
        [Required(ErrorMessage = "Role is Required")]
        public string Role { get; set; }
    }
}