using System.ComponentModel.DataAnnotations;
using Iep.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Iep.Models.View
{
    public class EditUser{
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
       
        public string email{get; set;}

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)] //Prilikom unosa nece se prikazivati tekst nego tackice :D
        public string password{get; set;}

        [Required]
        [Display(Name = "Confirm password")]
        [Compare(nameof(password), ErrorMessage = "Password and Confirm password fields must match!")] //Da mora da ima istu vrednost kao polje password; prvo je ime atribura sa kojom se uporedjuje i poruka greske
        [DataType(DataType.Password)]
        public string confirmPassword{get; set;}

    }
}