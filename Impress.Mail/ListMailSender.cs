using System.Collections.Generic;

namespace Impress.Mail
{
    public class ListMailSender : IMailSender
    {

        private List<IMailMessage> messages = new List<IMailMessage>();

        public void Send(IMailMessage message)
        {
            messages.Add(message);
        }


        public IList<IMailMessage> SendBox { get { return messages; } }
    }
}
