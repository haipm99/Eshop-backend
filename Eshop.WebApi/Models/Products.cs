using System;
using System.Collections.Generic;

namespace Eshop.WebApi.Models
{
    public partial class Products
    {
        public Products()
        {
            InvoicesDetails = new HashSet<InvoicesDetails>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ShopId { get; set; }
        public double UnitPrice { get; set; }
        public int StockAmmount { get; set; }
        public bool? Status { get; set; }

        public virtual Shops Shop { get; set; }
        public virtual ICollection<InvoicesDetails> InvoicesDetails { get; set; }
    }
}
