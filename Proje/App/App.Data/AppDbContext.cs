using App.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data
{
    public  class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.ApplyConfiguration(new CartItemEntityConfiguration());
            modelBuilder.ApplyConfiguration(new OrderEntityConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItemEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ProductCommentEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ProductEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ProductImageEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryEntityConfiguration());
            modelBuilder.ApplyConfiguration(new RoleEntityConfiguration());

            modelBuilder.Entity<RoleEntity>().HasData(
           new RoleEntity
           {
               Id = 1,
               Name = "Admin",
               CreatedAt = DateTime.Now
           },
           new RoleEntity
           {
               Id = 2,
               Name = "Seller",
               CreatedAt = DateTime.Now
           },
           new RoleEntity
           {
               Id = 3,
               Name = "Buyer",
               CreatedAt = DateTime.Now
           }
       );


            modelBuilder.Entity<UserEntity>().HasData(
                new UserEntity
                {
                    Id = 1,
                    Email = "admin@example.com",
                    FirstName = "Admin",
                    LastName = "User",
                    Password = "Admin123!",
                    RoleId = 1,
                    Enabled = true,
                    CreatedAt = DateTime.Now
                }
            );


            modelBuilder.Entity<CategoryEntity>().HasData(
                new CategoryEntity
                {
                    Id = 1,
                    Name = "Electronics",
                    Color = "FF5733",
                    IconCssClass = "fas fa-laptop",
                    CreatedAt = DateTime.Now
                },
                new CategoryEntity
                {
                    Id = 2,
                    Name = "Fashion",
                    Color = "33FF57",
                    IconCssClass = "fas fa-tshirt",
                    CreatedAt = DateTime.Now
                },
                new CategoryEntity
                {
                    Id = 3,
                    Name = "Books",
                    Color = "3357FF",
                    IconCssClass = "fas fa-book",
                    CreatedAt = DateTime.Now
                },
                new CategoryEntity
                {
                    Id = 4,
                    Name = "Home & Garden",
                    Color = "FF33F5",
                    IconCssClass = "fas fa-home",
                    CreatedAt = DateTime.Now
                },
                new CategoryEntity
                {
                    Id = 5,
                    Name = "Sports",
                    Color = "33FFF5",
                    IconCssClass = "fas fa-futbol",
                    CreatedAt = DateTime.Now
                },
                new CategoryEntity
                {
                    Id = 6,
                    Name = "Toys",
                    Color = "F5FF33",
                    IconCssClass = "fas fa-gamepad",
                    CreatedAt = DateTime.Now
                },
                new CategoryEntity
                {
                    Id = 7,
                    Name = "Health & Beauty",
                    Color = "FF3333",
                    IconCssClass = "fas fa-heart",
                    CreatedAt = DateTime.Now
                },
                new CategoryEntity
                {
                    Id = 8,
                    Name = "Automotive",
                    Color = "3333FF",
                    IconCssClass = "fas fa-car",
                    CreatedAt = DateTime.Now
                },
                new CategoryEntity
                {
                    Id = 9,
                    Name = "Pet Supplies",
                    Color = "33FF33",
                    IconCssClass = "fas fa-paw",
                    CreatedAt = DateTime.Now
                },
                new CategoryEntity
                {
                    Id = 10,
                    Name = "Food & Beverages",
                    Color = "FF9933",
                    IconCssClass = "fas fa-utensils",
                    CreatedAt = DateTime.Now
                }
            );

        }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<ProductImageEntity> ProductImages { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ProductCommentEntity> ProductComments { get; set; }
        public DbSet<OrderItemEntity> OrderItems { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<CartItemEntity> CartItems { get; set; }
        public DbSet<CartItemEntity> Cart { get; set; }
        public DbSet<ProductEntity> Product { get; set; }
        public DbSet<OrderEntity> Order { get; set; }
    }

    
}
