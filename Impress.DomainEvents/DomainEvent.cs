
using System;

namespace Impress.DomainEvents
{
    ///<summary>
    ///Represents a business rule event trigged by the domain layer (often in the service layer).
    ///DomainEvent implementations must be immutable. Once created, its state should not be modified. 
    ///Mutable implementations are not guaranteed to work because the DomainEventBus does not guarantees the same instance is passed to all listeners not it defines listener call order.
    ///</summary>
    public abstract class DomainEvent
    {
        public DateTime OccuredAt { get; private set; }
        protected DomainEvent(DateTime occuredAt)
        {
            OccuredAt = occuredAt;
        }
    }
}
