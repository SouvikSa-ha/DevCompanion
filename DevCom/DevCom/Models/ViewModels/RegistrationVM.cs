using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DevCom.Models.ViewModels
{
    public class RegistrationVM
    {
        [DataType(DataType.Text)]
        [Display(Name = "Username")]
        [Required(ErrorMessage = "This field is required")]
        public string Username { get; set; }


        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "This field is required")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "This field is required")]
        public string Password { get; set; }


        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "This field is required")]
        public string ConfirmPassword { get; set; }
    }
}