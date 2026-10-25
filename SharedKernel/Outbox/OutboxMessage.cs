using System;
using System.Collections.Generic;
using System.Text;

namespace SharedKernel.Outbox
{
    
        public class OutboxMessage
        {
            public Guid Id { get; init; } = Guid.NewGuid();
            public Guid TenantId { get; init; }
            public string Type { get; init; } = default!;
            public string Payload { get; init; } = default!;
            public string Status { get; set; } = "Pending";
            public int Attempts { get; set; }
            public DateTime NextAttemptAt { get; set; } = DateTime.UtcNow;
            public string? LastError { get; set; }
            public Guid? CorrelationId { get; set; }
            public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
            public DateTime? ProcessedAt { get; set; }
        }
}
