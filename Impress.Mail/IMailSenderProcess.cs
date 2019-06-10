namespace Impress.Mail
{
    public interface IMailSenderProcess : IMailSender
    {
        void Start();
        void Stop();
    }
}
