using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShopAdminApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        //for 1:M relationship
        public ShopApplicationUser OrderedBy { get; set; }
        public List<ProductsInOrder> Products { get; set; }
    }
}
