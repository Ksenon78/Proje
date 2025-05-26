using System.ComponentModel.DataAnnotations;
using App.Data.Entities;

namespace App.E_Ticaret.Models.ViewModels
{
    public class ProductViewModel
    {
        public int SellerId { get; set; }

        public List<CategoryDto> CategoryList { get; set; } = new();

        public int CategoryId { get; set; }
        public int? DiscountId { get; set; }

        [Required, MinLength(2), MaxLength(100)]
        public string Name { get; set; } = null!;

        public string Details { get; set; }


        [Required, DataType(DataType.Currency)]

        public decimal Price { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; } = null!;

        [Required]
        public byte StockAmount { get; set; } 

    }
}

