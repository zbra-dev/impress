using System;

namespace Impress.DomainEvents
{
    public sealed class UnexpectedDomainListenerException : DomainListenerException
    {
        public UnexpectedDomainListenerException(Type listener, Type domainEvent, Exception e)
            : base($"Unexpected error occured when DomainEvent {domainEvent.Name} was sent to {listener.Name} DomainListener", e)
        {
        }
    }
}
