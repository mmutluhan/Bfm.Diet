namespace Bfm.Diet.Core.Services
{
    public class MailConfiguration
    {
        public string Host { get; set; }
        public string MailAdress { get; set; }
        public string MailPassword { get; set; }
        public string MailUsername { get; set; }
        public int Port { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public bool UseSsl { get; set; }
    }
}