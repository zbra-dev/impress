using System;
using System.Collections.Generic;
using System.Linq;

namespace Impress.DomainEvents
{
    public class ListDomainEventBus : IDomainEventBus, IDomainListenerRegistry
    {
        private readonly ISet<DomainListenerDecorator> domainListeners = new HashSet<DomainListenerDecorator>();

        public ListDomainEventBus()
        {
            //no-op
        }

        public ListDomainEventBus(ICollection<IDomainListener> listeners)
        {
            AddListeners(listeners);
        }

        /// <summary>
        /// Search trought the registered list for IDomainListeners that are listening to that DomainEvent,
        /// and inform them to handle the DomainEvent.
        ///     <para>
        ///     Exceptions:
        ///         Throws ArgumentNullException if a null DomainEvent is sent.
        ///         Throws UnexpectedDomainListenerException if any Exception is thrown by registered DomainListener while handling the sent DomainEvent
        ///     </para>
        /// </summary>
        /// <param name="domainEvent">Event that will be called. Cannot be Null.</param>
        public void SendEvent(DomainEvent domainEvent)
        {
            if (domainEvent == null)
            {
                throw new ArgumentNullException("DomainEvent is required.");
            }

            foreach (var item in domainListeners)
            {
                try
                {
                    if (item.listener.IsListening(domainEvent))
                    {
                        item.listener.OnEvent(domainEvent);
                    }
                }
                catch (Exception e)
                {
                    throw new UnexpectedDomainListenerException(item.listener.GetType(), domainEvent.GetType(), e);
                }
            }
        }

        /// <summary>
        /// Registers an IDomainListener that will be listening to a DomainEvent.
        ///     <para>
        ///     Exceptions:
        ///          Throws ArgumentNullException if a null IDomainListener is added.
        ///          Throws DuplicatedDomainListenerException if a duplicated IDomainListener is added.
        ///     </para>
        /// </summary>
        /// <param name="listener">Listener that will be registered. Cannot be Null.</param>
        public void AddListener(IDomainListener listener)
        {
            if (listener == null)
            {
                throw new ArgumentNullException("IDomainListener is required.");
            }

            if (!domainListeners.Add(new DomainListenerDecorator(listener)))
            {
                throw new DuplicatedDomainListenerException(listener.GetType());
            }
        }

        /// <summary>
        /// Registers a collection of IDomainListener that will be listening to a DomainEvent.
        ///     <para>
        ///     Exceptions: 
        ///          Throws DuplicatedDomainListenerException if a duplicated IDomainListener is added.
        ///          Throws ArgumentNullException if a null ICollection<IDomainListener> is added.
        ///     </para>
        /// </summary>
        /// <param name="listener">Collection that will be registered. Cannot be null.</param>
        public void AddListeners(ICollection<IDomainListener> listeners)
        {
            if (listeners == null)
            {
                throw new ArgumentNullException("IDomainListener collection is required.");
            }

            foreach (var listener in listeners.Where(listener => listener != null))
            {
                AddListener(listener);
            }
        }
    }

}
