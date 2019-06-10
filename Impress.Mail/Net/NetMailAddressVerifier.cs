using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Impress.Mail.Net
{
    public class NetMailAddressVerifier : IMailAddressVerifier
    {

        private static readonly string CRLF = "\r\n";

        private string requesterEmail;
        private string requesterHost;

        public NetMailAddressVerifier() : this(new NetMailAddressVerifierConfiguration()) { }

        public NetMailAddressVerifier(NetMailAddressVerifierConfiguration configuration)
        {
            requesterHost = configuration.RequesterHost;
            requesterEmail = configuration.RequesterEmail;
        }

        private string[] DoMailServerLookup(string mailServerDomain)
        {
            Process compiler = new Process();
            compiler.StartInfo.FileName = "nslookup.exe";
            compiler.StartInfo.Arguments = "-type=mx " + mailServerDomain;
            compiler.StartInfo.UseShellExecute = false;
            compiler.StartInfo.RedirectStandardOutput = true;
            compiler.Start();

            var response = compiler.StandardOutput.ReadToEnd();

            compiler.WaitForExit();

            var preferences = new List<MXPreference>();
            string address;
            using (StringReader reader = new StringReader(response))
            {
                reader.ReadLine(); // first two are not relevant
                address = reader.ReadLine();

                int pos = address.IndexOf(":");
                if (pos > 0)
                {
                    address = address.Substring(pos + 1).Trim();
                }
                reader.ReadLine(); // separator

                var authorative = reader.ReadLine(); // autorative line or not found

                if (authorative == null)
                {
                    return new string[0]; // domain does not exist
                }

                string line;
                if (!authorative.Contains(":"))
                {
                    line = authorative;
                }
                else
                {
                    line = reader.ReadLine();
                }

                while (line.Length > 0)
                {
                    pos = line.IndexOf("=");
                    if (pos > 0)
                    {
                        var separator = line.IndexOf(",", pos + 1);
                        var rank = line.Substring(pos + 1, separator - pos - 1);
                        var exchanger = line.Substring(line.IndexOf("=", separator + 1) + 1).Trim();

                        preferences.Add(new MXPreference
                        {
                            Preference = double.Parse(rank),
                            Exchanger = exchanger
                        });
                    }


                    line = reader.ReadLine();
                }
            }

            preferences.Sort();

            if (!string.IsNullOrEmpty(requesterHost))
            {
                address = requesterHost;
            }

            return preferences.Count > 0 ? new string[] { address, preferences[0].Exchanger } : new string[0];
        }

        private bool DoVerify(string emailAddress, string mailServerUrl, string requesterHost)
        {
            try
            {
                using (TcpClient tClient = new TcpClient(mailServerUrl, 25))
                {
                    try
                    {

                        // byte[] dataBuffer;
                        string responseString;
                        using (NetworkStream netStream = tClient.GetStream())
                        {
                            StreamReader reader = new StreamReader(netStream);
                            responseString = reader.ReadLine();
                            /* Perform HELO to SMTP Server and get Response */
                            SendMailCommand(netStream, "HELO " + requesterHost);
                            responseString = reader.ReadLine();

                            if (responseString == null)
                            {
                                throw new MailVerificationException("HELO failed. Please verify RequesterHost to be valid.");
                            }
                            int? response = GetResponseCode(responseString);
                            if (response == 250)
                            {
                                SendMailCommand(netStream, "MAIL FROM:<" + requesterEmail + ">");
                                responseString = reader.ReadLine();

                                response = GetResponseCode(responseString);
                                if (response == 250)
                                {

                                    /* Read Response of the RCPT TO Message to know from server if it exist or not */
                                    SendMailCommand(netStream, "RCPT TO:<" + emailAddress.Trim() + ">");

                                    response = GetResponseCode(reader.ReadLine());

                                    /* QUITE CONNECTION */
                                    SendMailCommand(netStream, "QUITE");

                                    return !(response == 550);
                                }
                            }
                            throw new MailVerificationException(responseString);
                        }

                    }
                    catch (Exception e)
                    {
                        throw new MailVerificationException(e);
                    }
                }

            }
            catch (SocketException e)
            {
                //could not connect
                throw new MailVerificationException(e);
            }
            catch (Exception e)
            {
                throw new MailVerificationException(e);
            }

        }

        public bool VerifyExists(string emailAddress)
        {
            var pos = emailAddress.IndexOf("@");
            if (pos < 0)
            {
                return false;
            }
            var mailServerDomain = emailAddress.Substring(pos + 1);
            var server = DoMailServerLookup(mailServerDomain);

            if (server.Length == 0)
            {
                return false;
            }
            return DoVerify(emailAddress, server[1], server[0]);

        }

        private void SendMailCommand(NetworkStream netStream, string command)
        {
            var dataBuffer = BytesFromString(command + CRLF);
            netStream.Write(dataBuffer, 0, dataBuffer.Length);
        }

        private byte[] BytesFromString(string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }
        private int? GetResponseCode(string ResponseString)
        {
            return string.IsNullOrEmpty(ResponseString) ? (int?)null : int.Parse(ResponseString.Substring(0, 3));
        }
    }

    public class MXPreference : IComparable<MXPreference>
    {

        public double Preference { get; set; }

        public string Exchanger { get; set; }

        public int CompareTo(MXPreference other)
        {
            return this.Preference.CompareTo(other.Preference);
        }
    }
}
