using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using EShop.Domain.DomainModels;

namespace EShop.Domain.Relationships
{
    public class ProductsInOrder : BaseEntity
    {
        // join table - many to many relationship between products and orders

        [ForeignKey("ProductId")]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public int OrderId { get; set; }

        public Product Product { get; set; }
        public Order Order { get; set; }
        public int Quantity { get; set; }
    }
}
