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
    public class OrderEntity
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        

        [Required]
        [MinLength(2, ErrorMessage = "OrderCode en az 2 karakter olmalı.")]
        public string OrderCode { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Adres en az 2 karakter olmalı.")]
        [MaxLength(250, ErrorMessage = "Adres en fazla 250 karakter olabilir.")]
        public string Address { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<OrderItemEntity> OrderItems { get; set; } = new List<OrderItemEntity>();

        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public UserEntity User { get; set; }
        
    }

    public class OrderEntityConfiguration:IEntityTypeConfiguration<OrderEntity>
    {
        public void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(nameof(OrderEntity.OrderCode)).IsRequired();
            builder.Property(nameof(OrderEntity.Address)).HasMaxLength(250).IsRequired();
            builder.Property(nameof(OrderEntity.CreatedAt)).IsRequired();
            builder.HasOne(u => u.User).WithMany(u => u.UserOrders).HasForeignKey(u => u.UserId).OnDelete(DeleteBehavior.NoAction);

        }
    }
}
