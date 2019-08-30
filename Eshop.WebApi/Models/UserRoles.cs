using System;
using System.Collections.Generic;

namespace Eshop.WebApi.Models
{
    public partial class UserRoles
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }

        public virtual AppRoles Role { get; set; }
        public virtual AppUsers User { get; set; }
    }
}
