using System;

namespace Impress.DomainEvents
{
    public abstract class DomainListenerException : DomainEventBusException
    {
        protected DomainListenerException(string message) : base(message) { }
        protected DomainListenerException(string message, Exception e) : base(message, e) { }
    }
}
