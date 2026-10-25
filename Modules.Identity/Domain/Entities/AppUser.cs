using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Modules.Identity.Domain.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public Guid? TenantId { get; set; }
        public string FullName { get; set; } = default!;
    }
}
