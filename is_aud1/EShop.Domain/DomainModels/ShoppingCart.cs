using EShop.Domain.Relationships;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Domain.DomainModels
{
    public class ShoppingCart : BaseEntity
    {
        public string ApplictaionUserId { get; set; }
        public ICollection<ProductsInShoppingCart> ProductsInShoppingCarts { get; set; }
    }
}
