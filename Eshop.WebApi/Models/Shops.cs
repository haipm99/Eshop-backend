using System;
using System.Collections.Generic;

namespace Eshop.WebApi.Models
{
    public partial class Shops
    {
        public Shops()
        {
            Invoices = new HashSet<Invoices>();
            Products = new HashSet<Products>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Phone { get; set; }
        public bool? Status { get; set; }
        public int? UserId { get; set; }

        public virtual AppUsers User { get; set; }
        public virtual ICollection<Invoices> Invoices { get; set; }
        public virtual ICollection<Products> Products { get; set; }
    }
}
