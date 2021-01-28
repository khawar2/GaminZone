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

        [Display(Name = "UserName:")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Username is Required.")]
        [StringLength(50, MinimumLength = 3,
        ErrorMessage = "Username should be minimum 3 characters and a maximum of 50 characters")]
        public string UserName { get; set; }

        [Display(Name = "Email:")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email is Required.")]
        public string email { get; set; }


        [Display(Name = "Phone No.")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Phone no. is Required.")]
        [StringLength(15, MinimumLength = 7,
        ErrorMessage = "Phone No. should be minimum 7 numbers and a maximum of 15 characters")]
        public string phone { get; set; }

        [Required(ErrorMessage = "Phone Code is Required.")]
        public string phonecode { get; set; }


        [Required(ErrorMessage = "Age is Required.")]
        [MaxLength(3, ErrorMessage = "Age should be maximum of 3 characters. ")]
        [MinLength(2, ErrorMessage = "Age should be minimum of 2 characters. ")]
        public string Age { get; set; }



        [Display(Name = "Create Password:")]
        [Required(ErrorMessage = "Password is Required.")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 8,
        ErrorMessage = "Password should be minimum 8 characters and a maximum of 20 characters")]
        public string Password { get; set; }


        [Display(Name = "Confirm Password:")]
        [Required(ErrorMessage = "Confirm Password is Required.")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 8,
                  ErrorMessage = "Confirm Password should be minimum 8 characters and a maximum of 20 characters")]
        [Compare(nameof(Password), ErrorMessage = "Passwords don't match.")]
        public string confirmpassword { get; set; }

        [Display(Name = "Role:")]
        [Required(ErrorMessage = "Role is Required")]
        [UIHint("RolesComboBox")]
        public string Role { get; set; }
    }
}