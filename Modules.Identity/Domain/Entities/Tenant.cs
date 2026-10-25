using System;
using System.Collections.Generic;
using System.Text;

namespace Modules.Identity.Domain.Entities
{
    public class Tenant
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public string? CustomDomain { get; set; }
        public string Status { get; set; } = "Active";
        public string Plan { get; set; } = "Free";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
