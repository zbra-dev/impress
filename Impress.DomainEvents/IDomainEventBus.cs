namespace Impress.DomainEvents
{
    /// <summary>
    /// IDomainEventBus is responsible for dispatching DomainEvent instances to DomainListeners registered with the implementation of IDomainListenerRegistry.
    /// Dispatching is done in the same original thread that called the SendEvent method.
    /// The implementation does not have to send the same instance of the received event to all listeners (copies can be made). 
    /// Listeners are call in no particular order and may not be called always in the same order.
    /// </summary>
    public interface IDomainEventBus
    {
        void SendEvent(DomainEvent domainEvent);
    }
}
