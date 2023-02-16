using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using EShop.Domain.DomainModels;

namespace EShop.Domain.Relationships
{
    public class ProductsInShoppingCart : BaseEntity
    {
        public int ProductId { get; set; }
        public int CartId { get; set; }

        // references to the classes
        [ForeignKey("ProductId")]
        public Product Product { get; set; } 
        [ForeignKey("CartId")]
        public ShoppingCart ShoppingCart { get; set; }

        public Size ProductSize { get; set; }
        public int Quantity { get; set; }
    }
}
