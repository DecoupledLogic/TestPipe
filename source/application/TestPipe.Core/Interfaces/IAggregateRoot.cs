namespace TestPipe.Core.Interfaces
{
    using System;
    using System.Collections.Generic;

    public interface IAggregateRoot
    {
        Guid Id { get; }
        
        int Version { get; }

        int EventVersion { get; }

        void MarkChangesAsCommitted();

        void Replay(IEnumerable<dynamic> history);
    }
}