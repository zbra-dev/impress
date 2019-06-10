using System;

namespace Impress.Mail
{
    public class MailException : Exception
    {
        public MailException(System.Exception ex) : base(ex.Message, ex) { }

        public MailException(string message) : base(message) { }
    }
}
