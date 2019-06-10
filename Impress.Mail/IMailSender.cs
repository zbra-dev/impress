namespace Impress.Mail
{
    /// <summary>
    /// Executes the real sending of the message; normally on a network
    /// </summary>
    public interface IMailSender
    {
        /// <summary>
        /// Send mail message asynchronously. If the message is null, an InvalidMailMessage will be thrown
        /// </summary>
        /// <param name="message">The message to send</param>
        void Send(IMailMessage message);
    }
}
