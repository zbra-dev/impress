namespace Impress.Mail
{
    interface IMailAddressVerifier
    {

        bool VerifyExists(string mailAddress);
    }
}
