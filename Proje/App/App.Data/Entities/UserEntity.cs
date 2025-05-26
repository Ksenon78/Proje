using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Entities
{
    public class UserEntity
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [Required,MaxLength(50),MinLength(20)]
        public string FirstName { get; set; }
        [Required, MaxLength(50), MinLength(20)]
        public string LastName { get; set; }
        [Required, MaxLength(50), MinLength(20)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public int RoleId { get; set; }

        
        public bool Enabled { get; set; } = true;

        public DateTime CreatedAt { get; set; }

        public ICollection<ProductCommentEntity> UserProductComments { get; set; } = new List<ProductCommentEntity>();

        public ICollection<CartItemEntity> UserCartItems { get; set; } = new List<CartItemEntity>();

        public ICollection<OrderEntity> UserOrders { get; set; } = new List<OrderEntity>();

    
            
        [ForeignKey("RoleId")]
        public RoleEntity Role { get; set; }

    }

    public class UserEntityConfiguration:IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.Property(nameof(UserEntity.Email)).HasColumnType("nvarchar(255)").HasMaxLength(50).IsRequired();
            builder.Property(nameof(UserEntity.FirstName)).HasMaxLength(50).IsRequired();
            builder.Property(nameof(UserEntity.LastName)).HasMaxLength(50).IsRequired();
            builder.Property(nameof(UserEntity.Password)).HasColumnType("nvarchar(50)").IsRequired();
            builder.Property(nameof(UserEntity.Enabled)).HasDefaultValue(true).IsRequired();
            builder.Property(nameof(UserEntity.CreatedAt)).IsRequired();
            builder.HasOne(u => u.Role).WithMany(u=>u.Users).HasForeignKey(u=>u.RoleId).OnDelete(DeleteBehavior.NoAction);

        }
    }
}
