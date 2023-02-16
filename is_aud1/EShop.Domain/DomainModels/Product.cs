using EShop.Domain.Relationships;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Domain.DomainModels
{
    public class Product : BaseEntity
    {
        [Required]
        [Display(Name = "Ime na produkt")]
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public string ProductDescription { get; set; }
        public int ProductPrice { get; set; }
        public int ProductRating { get; set; }
        public ICollection<ProductsInShoppingCart> ProductsInShoppingCarts { get; set; }
    }
}
