using System;
using System.Collections.Generic;

namespace Eshop.WebApi.Models
{
    public partial class InvoicesDetails
    {
        public int Id { get; set; }
        public int? InvoiceId { get; set; }
        public int? ProductId { get; set; }
        public int Quantity { get; set; }
        public double PaidAmmount { get; set; }

        public virtual Invoices Invoice { get; set; }
        public virtual Products Product { get; set; }
    }
}
