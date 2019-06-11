using System;

namespace Impress.DomainEvents
{
    public abstract class DomainEventBusException : Exception
    {
        protected DomainEventBusException(string message) : base(message) { }
        protected DomainEventBusException(string message, Exception e) : base(message, e) { }
    }
}
