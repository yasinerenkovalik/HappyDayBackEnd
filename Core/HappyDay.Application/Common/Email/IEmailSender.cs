using System.Threading;
using System.Threading.Tasks;

namespace HappyDay.Application.Common.Email
{
    public interface IEmailSender
    {
        Task SendAsync(
            string to,
            string subject,
            string htmlBody,
            string? textBody = null,
            CancellationToken ct = default
        );
    }
}