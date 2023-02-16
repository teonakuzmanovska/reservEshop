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
        [Display(Name = "Product name")]
        public string ProductName { get; set; }
        [Display(Name = "Product Image")]
        public string ProductImage { get; set; }
        [Display(Name = "Product specifications")]
        public string ProductDescription { get; set; }
        [Display(Name = "Rezervation fee")]
        public int ProductPrice { get; set; }
        public ICollection<ProductsInShoppingCart> ProductsInShoppingCarts { get; set; }
    }
}
