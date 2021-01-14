using System;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Bfm.Diet.Core.Extensions;

namespace Bfm.Diet.Core.Services
{
    public class MailService : IMailService
    {
        private readonly string _host;
        private readonly bool _isMailerReady = true;
        private readonly string _mailAddress;
        private readonly string _mailPassword;
        private readonly string _mailUsername;
        private readonly int _port;
        private readonly bool _useDefaultCredentials;
        private readonly bool _useSsl;

        public MailService(MailConfiguration mailConfig)
        {
            _port = mailConfig.Port;
            _mailUsername = mailConfig.MailUsername;
            _mailAddress = mailConfig.MailAdress ?? mailConfig.MailUsername;
            _mailPassword = mailConfig.MailPassword;
            _host = mailConfig.Host;
            _useSsl = mailConfig.UseSsl;
            _useDefaultCredentials = mailConfig.UseDefaultCredentials;
        }

        public Task SendAsync(string to, string[] attachments, string subject, string body = "")
        {
            if (!_isMailerReady)
                return Task.CompletedTask;

            var userState = to.MaskEmail();
            var fromAddress = new MailAddress(_mailAddress, "Yardim Habercisi");
            var toAddress = new MailAddress(to);
            var mailMessage = new MailMessage(fromAddress.Address, toAddress.Address, subject, body);

            if (attachments != null)
                foreach (var attachment in attachments)
                    mailMessage.Attachments.Add(new Attachment(attachment));

            var cli = new SmtpClient
            {
                Port = _port,
                Host = _host,
                EnableSsl = _useSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = _useDefaultCredentials,
                Credentials = new NetworkCredential(_mailUsername, _mailPassword)
            };


            cli.SendCompleted += SendCompletedCallback;
            ThreadPool.QueueUserWorkItem(x => { cli.SendAsync(mailMessage, userState); });
            return Task.CompletedTask;
        }

        private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                Console.Write($"E-Mail to {e.UserState} was canceled . ");
                if (e.Error != null)
                    Console.WriteLine($"canceled e-mail to {e.UserState} was error : {e.Error.Message} ");
                return;
            }

            if (e.Error != null)
            {
                Console.WriteLine($"E-Mail to {e.UserState} was error . {e.Error.Message} ");
                return;
            }

            Console.WriteLine($"Sending e-mail to {e.UserState} completed. ");
        }
    }
}