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
    public class ProductEntity
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int SellerId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; } = null!;
        [DataType(DataType.Currency)]
        [Range(2,10000)]
        public decimal Price { get; set; }
        public string Description { get; set; } = null!;

        public string Details { get; set; } 
        public byte StockAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Enabled { get; set; }

        public ICollection<ProductImageEntity> Images { get; set; } = new List<ProductImageEntity>();
        public ICollection<ProductCommentEntity> ProductComments { get; set; } = new List<ProductCommentEntity>();

        public ICollection<CartItemEntity> ProductCartItems { get; set; } = new List<CartItemEntity>();

        public ICollection<OrderItemEntity> ProductOrderItems { get; set; } = new List<OrderItemEntity>();

        [ForeignKey(nameof(SellerId))]
        public UserEntity Seller { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public CategoryEntity Category { get; set; }

    }

        public class ProductEntityConfiguration:IEntityTypeConfiguration<ProductEntity>
        {
          public void Configure(EntityTypeBuilder<ProductEntity> builder)
          {
            builder.Property(nameof(ProductEntity.SellerId)).IsRequired();
            builder.Property(nameof(ProductEntity.CategoryId)).IsRequired();
            builder.Property(nameof(ProductEntity.Name)).IsRequired().HasMaxLength(30);
            builder.Property(nameof(ProductEntity.Price)).IsRequired();
            builder.Property(nameof(ProductEntity.Description)).IsRequired();
            builder.Property(nameof(ProductEntity.Details)).IsRequired().HasMaxLength(100);
            builder.Property(nameof(ProductEntity.CreatedAt)).IsRequired();
            builder.Property(nameof(ProductEntity.Enabled)).IsRequired().HasDefaultValue(true);
            //No action koymamızın sebebi bir kullanıcı(bu senaryoda bir satıcı) silindiğinde ona bağlı ürünlerin
            //database'den silinmesi engellenir.
            builder.HasOne(p => p.Seller).WithMany().HasForeignKey(p => p.SellerId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(p => p.Category).WithMany().HasForeignKey(p => p.CategoryId).OnDelete(DeleteBehavior.NoAction);
          }
        }
}
