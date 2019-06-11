namespace Impress.DomainEvents
{
    ///<summary>
    /// The IDomainListener defines which DomainEvent it is listened to, and how it will be handled.
    /// IDomainListener should only handle events that it is listening to.
    ///</summary>
    public interface IDomainListener
    {
        bool IsListening(DomainEvent domainEvent);

        void OnEvent(DomainEvent domainEvent);
    }
}
