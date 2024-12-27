using System.Net.Mail;
using System.Net;
using CoreAPI_V3.Models;
using CoreAPI_V3.Interface;

namespace CoreAPI_V3.CommonMethods
{
    public class CommonHelper
    {
        private readonly ILoggerService _loggerService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public CommonHelper(ILoggerService loggerService, IEmailService emailService, IConfiguration configuration)
        {
            _loggerService = loggerService;
            _emailService = emailService;
            _configuration = configuration;
        }
        public CommonHelper()
        {
            
        }
        public void SendErrorToText(string foldername, string controllername, string methodname, Exception ex)
        {
            _loggerService.LogError(foldername, controllername, methodname, ex);

            var emailSettings = _configuration.GetSection("Logging");
            string enableEmail = emailSettings["EnableEmailLog"];

            string emailContent = "<br/>------------------------------------------------------------------------" +
                             $"<br/>Time: {DateTime.Now:MM/dd/yyyy HH:mm:ss tt}" +
                             $"<br/>Folder Name: {foldername}" +
                             $"<br/>Controller Name: {controllername}" +
                             $"<br/>Method Name: {methodname}" +
                             $"<br/>Trace Info: {ex}" +
                             "<br/>-------------------------------------------------------------------------------";
            string recipient = _configuration["EmailSettings:MailRecipient"];
            bool enableEmailLog = Convert.ToBoolean(_configuration["Logging:EnableEmailLog"]);
            if (enableEmailLog)
            {
                _emailService.SendEmailLog(recipient, $"Log: {methodname} System: {Environment.MachineName}", emailContent);
            }
        }

        public static string Getlatlong(string lat, ref string retmsg)
        {
            string lcsecs = "";
            decimal lalatlong;
            try
            {
                string[] splideg = lat.Split(new char[1] { 'D' });
                string lcdeg = splideg[0];
                string lcmins = splideg[1];
                string[] splimins = lcmins.Split(new char[1] { 'M' });
                lcmins = splimins[0];
                try
                {
                    lcsecs = splimins[1];
                }
                catch (Exception)
                {
                    lcsecs = lcmins;
                    lcmins = "0";
                }
                if (!(lcsecs == ""))
                {
                    string[] splisecs = lcsecs.Split(new char[1] { 'S' });
                    lcsecs = splisecs[0];
                }
                decimal lcdegg = decimal.Parse(lcdeg);
                decimal lcminss = ((lcmins == "") ? Convert.ToDecimal(0.0) : (Convert.ToDecimal(lcmins) / Convert.ToDecimal(60.0)));
                decimal lcsecss = ((lcsecs == "") ? Convert.ToDecimal(0.0) : (Convert.ToDecimal(lcsecs) / Convert.ToDecimal(3600.0)));
                lalatlong = lcdegg + lcminss + lcsecss;
            }
            catch (Exception)
            {
                retmsg = null;
                return retmsg;
            }
            return decimal.Round(lalatlong, 6).ToString();
        }

        public static string Getlatlongb(string lat, ref string retmsg)
        {
            string lcsecs = "";
            decimal lalatlong;
            try
            {
                string[] splideg = lat.Split(new char[1] { 'D' });
                string lcdeg = splideg[0];
                string lcmins = splideg[1];
                string[] splimins = lcmins.Split(new char[1] { 'M' });
                lcmins = splimins[0];
                try
                {
                    lcsecs = splimins[1];
                }
                catch (Exception)
                {
                    lcsecs = lcmins;
                    lcmins = "0";
                }
                if (!(lcsecs == ""))
                {
                    string[] splisecs = lcsecs.Split(new char[1] { 'S' });
                    lcsecs = splisecs[0];
                }
                decimal lcdegg = decimal.Parse(lcdeg);
                if (lcdeg.Contains("-13"))
                {
                    string[] spli = lcdeg.Split(new char[1] { '-' });
                    lcdegg = decimal.Parse(spli[1]);
                }
                decimal lcminss = ((lcmins == "") ? Convert.ToDecimal(0.0) : (Convert.ToDecimal(lcmins) / Convert.ToDecimal(60.0)));
                decimal lcsecss = ((lcsecs == "") ? Convert.ToDecimal(0.0) : (Convert.ToDecimal(lcsecs) / Convert.ToDecimal(3600.0)));
                string lalatlongs = "-" + (lcdegg + lcminss + lcsecss);
                lalatlong = decimal.Parse(lalatlongs);
            }
            catch (Exception)
            {
                retmsg = null;
                return retmsg;
            }
            return decimal.Round(lalatlong, 6).ToString();
        }

        public static DashboardModel.ResponseMessages Success(string msg)
        {
            DashboardModel.ResponseMessages resp = new DashboardModel.ResponseMessages();
            resp.status = "Ok";
            resp.statuscode = 200;
            resp.message = msg;
            return resp;
        }

        public static DashboardModel.ResponseMessages Error(string msg)
        {
            DashboardModel.ResponseMessages resp = new DashboardModel.ResponseMessages();
            resp.status = "Error";
            resp.statuscode = 500;
            resp.message = msg;
            return resp;
        }

        public static DashboardModel.ResponseMessages NoContent(string msg)
        {
            DashboardModel.ResponseMessages resp = new DashboardModel.ResponseMessages();
            resp.status = "No Content";
            resp.statuscode = 204;
            resp.message = msg;
            return resp;
        }

        public static DashboardModel.ResponseMessages UnAuthorized(string msg)
        {
            DashboardModel.ResponseMessages resp = new DashboardModel.ResponseMessages();
            resp.status = "UnAuthorized";
            resp.statuscode = 401;
            resp.message = msg;
            return resp;
        }
        public static DashboardModel.ResponseMessages BadRequest(string msg)
        {
            DashboardModel.ResponseMessages resp = new DashboardModel.ResponseMessages();
            resp.status = "Bad Request";
            resp.statuscode = 400;
            resp.message = msg;
            return resp;
        }
    }
}
