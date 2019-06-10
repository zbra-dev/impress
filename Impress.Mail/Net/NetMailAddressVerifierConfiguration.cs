namespace Impress.Mail.Net
{
    public class NetMailAddressVerifierConfiguration
    {

        public string RequesterEmail { get; set; }
        public string RequesterHost { get; set; }

        public NetMailAddressVerifierConfiguration()
        {
            this.RequesterEmail = "YourGmailIDHere@gmail.com";
        }

    }
}
