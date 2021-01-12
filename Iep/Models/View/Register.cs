using System.ComponentModel.DataAnnotations;
using Iep.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Iep.Models.View
{
    public class RegisterModel{
        [Required]
        [Display(Name = "First name")]
        public string firstName{get; set;}

        [Required]
        [Display(Name = "Last name")]
        public string lastName{get; set;}

        [Required]
        [Display(Name = "Gender")]
        public char gender{get; set;}

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Remote(controller: "User", action:nameof(UserController.isEmailUnique))] 
        public string email{get; set;}

        [Required]
        [Display(Name = "Username")]
        [MinLength(6)]
        [Remote(controller: "User", action:nameof(UserController.isUsernameUnique))]
        public string username{get; set;}

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)] 
        public string password{get; set;}

        [Required]
        [Display(Name = "Confirm password")]
        [Compare(nameof(password), ErrorMessage = "Password and Confirm password fields must match!")] 
        [DataType(DataType.Password)]
        public string confirmPassword{get; set;}

    }
}