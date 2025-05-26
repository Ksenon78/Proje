using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Entities
{
    public class ProductCommentEntity
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required,MaxLength(200),MinLength(20)]
        public string Text { get; set; } = string.Empty;

        [Range(1, 5, ErrorMessage = "Yıldız sayısı 1 ile 5 arasında olmalıdır.")]
        public byte StarCount { get; set; }


        [Required]
        public bool IsConfirmed { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public UserEntity User { get; set; } 

        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public ProductEntity Product { get; set; } 
    }

    public class ProductCommentEntityConfiguration:IEntityTypeConfiguration<ProductCommentEntity>
    {
        public void Configure(EntityTypeBuilder<ProductCommentEntity> builder)
        {
            builder.Property(nameof(ProductCommentEntity.Text)).HasMaxLength(200).IsRequired();
            builder.Property(nameof(ProductCommentEntity.IsConfirmed)).HasDefaultValue(true).IsRequired();
            builder.Property(nameof(ProductCommentEntity.CreatedAt)).IsRequired();
            builder.HasOne(u => u.User).WithMany(u => u.UserProductComments).HasForeignKey(u => u.UserId).IsRequired();
            builder.HasOne(p => p.Product).WithMany(p => p.ProductComments).HasForeignKey(p => p.ProductId).IsRequired();
        }
    }
}
