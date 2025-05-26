using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Entities
{
    public class CategoryEntity
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required,MinLength(20),MaxLength(50)]
        public string Name { get; set; } 
        [Required,MinLength(20),MaxLength(50)]
        [RegularExpression("^#(?:[0-9a-fA-F]{3}){1,2}$", ErrorMessage = "Geçerli bir HEX renk kodu girin. Örn: #FF5733")]
        public string Color { get; set; }
        [Required(ErrorMessage = "Icon sınıfı zorunludur.")]
        [MaxLength(100, ErrorMessage = "Icon sınıfı en fazla 100 karakter olabilir.")]
        public string IconCssClass { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

     public class CategoryEntityConfiguration : IEntityTypeConfiguration<CategoryEntity>
    {
        public void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            builder.Property(nameof(CategoryEntity.Name)).IsRequired();
            builder.Property(nameof(CategoryEntity.Color)).HasMaxLength(50).IsRequired();
            builder.Property(nameof(CategoryEntity.IconCssClass)).HasMaxLength(100).IsRequired();
            builder.Property(nameof(CategoryEntity.CreatedAt)).IsRequired();
        }
    }
}
