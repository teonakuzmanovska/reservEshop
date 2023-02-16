using EShop.Domain.DomainModels;
using EShop.Domain;
using EShop.Domain.Identity;
using EShop.Domain.Relationships;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Repository
{
    public class ApplicationDbContext : IdentityDbContext<ShopApplicationUser>
    {
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<ProductsInShoppingCart> ProductsInShoppingCarts { get; set; }
        public virtual DbSet<ShopApplicationUser> ShopApplicationUsers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<ProductsInOrder> ProductsInOrders { get; set; }
        public virtual DbSet<EmailMessage> EmailMessages { get; set; }

        // this is in order to obtain composite key for ProductsInShoppingCarts
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ProductsInShoppingCart>().HasKey(c => new { c.CartId, c.ProductId });
            builder.Entity<ProductsInOrder>().HasKey(c => new { c.OrderId, c.ProductId });
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
