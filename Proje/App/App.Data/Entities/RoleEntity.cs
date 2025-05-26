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
    public  class RoleEntity
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required,MaxLength(50),MinLength(20)]
        public string Name { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

        public ICollection<UserEntity> Users { get; set; } = new List<UserEntity>();
    }

    public class RoleEntityConfiguration:IEntityTypeConfiguration<RoleEntity>
    {
        public void Configure(EntityTypeBuilder<RoleEntity> builder)
        {
            builder.Property(nameof(RoleEntity.Name)).HasMaxLength(50).IsRequired();
            builder.Property(nameof(RoleEntity.CreatedAt)).IsRequired();

        }
    }
}
