using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Entities
{
    public class CartItemEntity
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Range(1, 100, ErrorMessage = "Adet en az 1 olmalı.")]
        public byte Quantity { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public UserEntity User { get; set; } 

        [ForeignKey(nameof(ProductId))]
        public int ProductId { get; set; }


        public ProductEntity Product { get; set; } 
        
        public int CartId { get; set; }
        [ForeignKey(nameof(CartId))]
        public CartEntity Cart { get; set; }



    }

    public class CartItemEntityConfiguration : IEntityTypeConfiguration<CartItemEntity>
    {
        public void Configure(EntityTypeBuilder<CartItemEntity> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(nameof(CartItemEntity.Quantity)).IsRequired();
            builder.Property(nameof(CartItemEntity.CreatedAt)).IsRequired();
            builder.HasOne(c => c.Cart).WithMany(c => c.ProductCartItems).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(u => u.User).WithMany(u => u.UserCartItems).HasForeignKey(u => u.UserId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(p => p.Product).WithMany(p => p.ProductCartItems).HasForeignKey(p => p.ProductId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
