using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GamingZone.ViewModels
{
    public class Login
    {
        [Display(Name = "Email:")]
        [Required(ErrorMessage = "User Name is Required.")]
        [DataType(DataType.Text)]
        [StringLength(20, ErrorMessage = "Do not enter more than 20 characters.")]
        public string UserName { get; set; }


        [Display(Name = "Password:")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is Required.")]
        [MinLength(8, ErrorMessage = "Do not enter less then 8 characters")]
        [MaxLength(20, ErrorMessage = "Do not enter more than 20 characters.")]
        public string Password { get; set; }

    }
}