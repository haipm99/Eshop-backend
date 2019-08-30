using System;
using System.Collections.Generic;

namespace Eshop.WebApi.Models
{
    public partial class AppRoles
    {
        public AppRoles()
        {
            UserRoles = new HashSet<UserRoles>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string DisplayName { get; set; }

        public virtual ICollection<UserRoles> UserRoles { get; set; }
    }
}
