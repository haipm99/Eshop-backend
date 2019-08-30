using System;
using System.Collections.Generic;

namespace Eshop.WebApi.Models
{
    public partial class AppUsers
    {
        public AppUsers()
        {
            Invoices = new HashSet<Invoices>();
            Shops = new HashSet<Shops>();
            UserRoles = new HashSet<UserRoles>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Invoices> Invoices { get; set; }
        public virtual ICollection<Shops> Shops { get; set; }
        public virtual ICollection<UserRoles> UserRoles { get; set; }
    }
}
