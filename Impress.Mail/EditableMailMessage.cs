using System;
using System.Collections.Generic;
using System.Text;

namespace Impress.Mail
{
    public class EditableMailMessage : IMailMessage
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string ReplyTo { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtmlBody { get; set; }

        public IReadOnlyList<IMailAttachment> Attachments { get { return attachements; } }

        private List<IMailAttachment> attachements = new List<IMailAttachment>();

        public void AddAttachment(IMailAttachment attachment)
        {
            attachements.Add(attachment);
        }

        public void RemoveAttachment(IMailAttachment attachment)
        {
            attachements.Remove(attachment);
        }

        public void ClearAttachments()
        {
            attachements.Clear();
        }

        public bool HasAttachments { get { return attachements.Count > 0; } }
    }
}
