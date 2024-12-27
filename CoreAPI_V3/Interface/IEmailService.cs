using System.Net.Mail;
using System.Net;

namespace CoreAPI_V3.Interface
{
    public interface IEmailService
    {
        bool SendEmailLog(string emailId, string subject, string messageBody);
    }
}
 