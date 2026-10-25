using System;
using System.Collections.Generic;
using System.Text;

namespace SharedKernel.Events
{
    public interface IIntegrationEvent
    {
        Guid EventId { get; }
        Guid TenantId { get; }
        DateTime OccurredAt { get; }
    }
}
