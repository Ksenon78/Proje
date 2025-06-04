using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using App.Data.Entities;

namespace App.E_Ticaret.Models.ViewModels
{
    public class OrderCreateViewModel
    {
        [Required(ErrorMessage = "Adres alanı zorunludur.")]
        [MinLength(2, ErrorMessage = "Adres en az 2 karakter olmalıdır.")]
        [StringLength(500, ErrorMessage = "Adres en fazla 500 karakter olabilir.")]
        public string Address { get; set; }

        public List<CartItemEntity> CartItems { get; set; } = new();
    }
}
