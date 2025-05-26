using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Entities
{
    public class OrderItemEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }



        [Required]
        [Range(1, byte.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public byte Quantity { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal UnitPrice { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public int OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public OrderEntity Order { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public ProductEntity Product { get; set; }
        

    }

    public class OrderItemEntityConfiguration:IEntityTypeConfiguration<OrderItemEntity>
    {
        public void Configure(EntityTypeBuilder<OrderItemEntity> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(nameof(OrderItemEntity.Quantity)).IsRequired();
            builder.Property(nameof(OrderItemEntity.UnitPrice)).IsRequired();
            builder.Property(nameof(OrderItemEntity.CreatedAt)).IsRequired();
            builder.HasOne(u => u.Order).WithMany(u => u.OrderItems).HasForeignKey(u => u.OrderId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(p => p.Product).WithMany(p => p.ProductOrderItems).HasForeignKey(p => p.ProductId).OnDelete(DeleteBehavior.NoAction);


        }
    }
    
}
