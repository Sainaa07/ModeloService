using System;
using System.Collections.Generic;
using System.Text;

namespace SharedKernel.Tenancy
{
    public interface ITenantContext
    {
        Guid? TenantId { get; }
        string Slug { get; }
        bool IsPlatform { get; }
    }
}
