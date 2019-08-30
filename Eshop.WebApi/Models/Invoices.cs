using System;
using System.Collections.Generic;

namespace Eshop.WebApi.Models
{
    public partial class Invoices
    {
        public Invoices()
        {
            InvoicesDetails = new HashSet<InvoicesDetails>();
        }

        public int Id { get; set; }
        public DateTime Time { get; set; }
        public int? TotalPaidAmmount { get; set; }
        public int? ShopId { get; set; }
        public int? UserId { get; set; }

        public virtual Shops Shop { get; set; }
        public virtual AppUsers User { get; set; }
        public virtual ICollection<InvoicesDetails> InvoicesDetails { get; set; }
    }
}
