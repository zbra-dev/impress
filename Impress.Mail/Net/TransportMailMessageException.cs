namespace Impress.Mail.Net
{
    public class TransportMailMessageException : MailException
    {
        public TransportMailMessageException(System.Exception ex) : base(ex) { }
    }
}
