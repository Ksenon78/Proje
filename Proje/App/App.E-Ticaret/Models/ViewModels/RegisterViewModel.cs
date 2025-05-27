using System.ComponentModel.DataAnnotations;

namespace App.E_Ticaret.Models.ViewModels
{
    public class RegisterViewModel
    {
        
        [Required(ErrorMessage = "Email alanı zorunludur")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre alanı zorunludur")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Şifre tekrar alanı zorunludur")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor")]
        public string ConfirmPassword { get; set; }
    }

}

