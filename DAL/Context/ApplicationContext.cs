using System;
using System.Collections.Generic;
using System.Text;
using BLL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DAL.Context
{
    public class ApplicationContext : IdentityDbContext<User, Role, Guid, UserClaims, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {
            
        }
        //public DbSet<Book> Books { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Variant> Variants { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Availability> Availabilities { get; set; }
        //public DbSet<ProductsGroup> ProductGroups { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<ProductsGroup>((pc =>
            //{
            //    pc.HasNoKey();
            //    pc.ToView("View_ProductGroups");
            //}));

            //modelBuilder.Entity<Cart>()
            //    .HasOne(a => a.User)
            //    .WithOne(b => b.Cart)
            //    .HasForeignKey<AuthorBiography>(b => b.AuthorRef);
            modelBuilder.Entity<CartItem>()
                .HasKey(bc => new { bc.CartId, bc.AvailabilityId });
            modelBuilder.Entity<CartItem>()
                .HasOne(bc => bc.Cart)
                .WithMany(b => b.CartItems)
                .HasForeignKey(bc => bc.CartId);
            modelBuilder.Entity<CartItem>()
                .HasOne(bc => bc.Availability)
                .WithMany(c => c.CartItems)
                .HasForeignKey(bc => bc.AvailabilityId);
            //modelBuilder.Entity<Order>()
            //    .HasOne(a => a.User)
            //    .WithOne(b => b.Order)
            //    .HasForeignKey<User>(b => b.OrderId);
            modelBuilder.Entity<OrderItem>()
                .HasKey(bc => new { bc.OrderId, bc.AvailabilityId });
            modelBuilder.Entity<OrderItem>()
                .HasOne(bc => bc.Order)
                .WithMany(b => b.OrderItems)
                .HasForeignKey(bc => bc.OrderId);
            modelBuilder.Entity<OrderItem>()
                .HasOne(bc => bc.Availability)
                .WithMany(c => c.OrderItems)
                .HasForeignKey(bc => bc.AvailabilityId);
        }
    }
}
