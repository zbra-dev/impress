namespace Impress.Mail.Net
{
    public class NetSenderConfiguration
    {
        public bool Enabled { get; set; }
        public bool EnableSsl { get; set; }

        public NetSenderConfiguration()
        {
            Enabled = true;
            EnableSsl = false;
        }
    }
}
