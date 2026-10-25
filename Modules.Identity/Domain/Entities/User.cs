using System;
using System.Collections.Generic;
using System.Text;

namespace Modules.Identity.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Email { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
