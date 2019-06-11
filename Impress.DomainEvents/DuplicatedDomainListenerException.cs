using System;
using System.Collections.Generic;
using System.Text;

namespace Impress.DomainEvents
{
    public sealed class DuplicatedDomainListenerException : DomainListenerException
    {
        public DuplicatedDomainListenerException(Type duplicatedListener)
            : base($"Cannot register {duplicatedListener.Name} listener twice.")
        {
        }
    }
}
