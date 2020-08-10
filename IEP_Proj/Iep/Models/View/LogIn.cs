using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Iep.Models.View
{
    public class LogInModel
    {
        [Required]
        [Display(Name ="Username")]
        public string username {get; set;}

        [Required]
        [DataType(DataType.Password)]
        [Display(Name ="Password")]
        public string password {get; set;}

        [HiddenInput]
        public string returnUrl {get; set;}
    }


}