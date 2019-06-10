using System;
using System.Collections.Generic;
using System.Text;

namespace Impress.Mail.Net
{
    public class AuthenticationRequiredMailMessage : MailException
    {
        public AuthenticationRequiredMailMessage(System.Exception ex) : base(ex) { }
    }
}
