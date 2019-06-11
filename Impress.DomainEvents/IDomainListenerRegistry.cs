using System.Collections.Generic;

namespace Impress.DomainEvents
{
    /// <summary>
    /// The IDomainListenerRegistry will register one or multiple IDomainListener that handles DomainEvents.
    /// </summary>
    interface IDomainListenerRegistry
    {
        void AddListener(IDomainListener listener);
        void AddListeners(ICollection<IDomainListener> listeners);
    }
}
