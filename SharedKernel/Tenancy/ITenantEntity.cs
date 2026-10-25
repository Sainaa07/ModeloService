using System;
using System.Collections.Generic;
using System.Text;

namespace SharedKernel.Tenancy
{
    public interface ITenantEntity
    {
        Guid TenantId { get; }
    }
}
