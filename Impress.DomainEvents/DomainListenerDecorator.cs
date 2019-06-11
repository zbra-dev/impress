namespace Impress.DomainEvents
{
    internal sealed class DomainListenerDecorator
    {
        public readonly IDomainListener listener;

        public DomainListenerDecorator(IDomainListener domainListener)
        {
            this.listener = domainListener;
        }

        public override bool Equals(object obj)
        {
            var other = obj as DomainListenerDecorator;

            return other != null
                && (other.listener.GetType().IsInstanceOfType(this.listener)
                    || this.listener.GetType().IsInstanceOfType(other.listener)
                );
        }

        public override int GetHashCode()
        {
            //Due to parent and child classes are being considered equal, it was not possible to use a more efficient option.
            return 0;
        }
    }
}
