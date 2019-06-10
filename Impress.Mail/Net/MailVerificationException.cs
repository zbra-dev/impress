using System;

namespace Impress.Mail.Net
{
    public class MailVerificationException : MailException
    {
        internal MailVerificationException(string message) : base(message) { }
        internal MailVerificationException(Exception ex) : base(ex) { }
    }
}
