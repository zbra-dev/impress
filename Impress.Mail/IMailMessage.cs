using System.Collections.Generic;

namespace Impress.Mail
{
    /// <summary>
    /// Read only Message Data
    /// </summary>
    public interface IMailMessage
    {

        string From { get; }
        string To { get; }
        string Cc { get; }
        string ReplyTo { get; }
        string Bcc { get; }
        string Subject { get; }
        string Body { get; }
        bool IsHtmlBody { get; }

        IReadOnlyList<IMailAttachment> Attachments { get; }
    }
}
