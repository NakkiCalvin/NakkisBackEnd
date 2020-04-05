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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Cart>()
            //    .HasOne(a => a.User)
            //    .WithOne(b => b.Cart)
            //    .HasForeignKey<AuthorBiography>(b => b.AuthorRef);
            modelBuilder.Entity<CartItem>()
                .HasKey(bc => new { bc.CartId, bc.ProductId });
            modelBuilder.Entity<CartItem>()
                .HasOne(bc => bc.Cart)
                .WithMany(b => b.CartItems)
                .HasForeignKey(bc => bc.CartId);
            modelBuilder.Entity<CartItem>()
                .HasOne(bc => bc.Product)
                .WithMany(c => c.CartItems)
                .HasForeignKey(bc => bc.ProductId);
        }
    }
}
