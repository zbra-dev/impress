using System;
using System.Collections.Generic;
using System.Text;

namespace Impress.Mail.Net
{
    public class TimeoutTransportMailException : TransportMailMessageException
    {
        public TimeoutTransportMailException(System.Exception ex) : base(ex) { }
    }
}
