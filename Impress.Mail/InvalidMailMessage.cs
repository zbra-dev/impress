namespace Impress.Mail
{
    public class InvalidMailMessage : MailException
    {
        public InvalidMailMessage(System.Exception ex) : base(ex) { }

        public InvalidMailMessage(string message) : base(message) { }
    }
}
