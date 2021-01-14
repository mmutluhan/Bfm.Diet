using System.Threading.Tasks;

namespace Bfm.Diet.Core.Services
{
    public interface IMailService
    {
        Task SendAsync(string to, string[] attachments, string subject, string body = "");
    }
}