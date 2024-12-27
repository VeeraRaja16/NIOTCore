using CoreAPI_V3.Interface;

namespace CoreAPI_V3.Repository
{
    public class LoggerService : ILoggerService
    {
        private readonly IConfiguration _configuration;

        public LoggerService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void LogError(string foldername, string controllername, string methodname, Exception ex)
        {
            string line = Environment.NewLine + Environment.NewLine;
            string errorlineNo = ex.StackTrace?.Substring(ex.StackTrace.Length - 7, 7);
            string errormsg = ex.GetType().Name;
            string extype = ex.GetType().ToString();
            string exurl = AppDomain.CurrentDomain.ToString();
            string errorLocation = ex.Message;

            string logFolderPath = _configuration["Logging:LogFolderPath"] ?? "Log/";
            string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, logFolderPath);

            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }

            filepath = Path.Combine(filepath, DateTime.Today.ToString("dd-MM-yy") + ".txt");

            using (StreamWriter sw = File.AppendText(filepath))
            {
                string error = $"Log Written Date: {DateTime.Now}{line}Error Line No : {errorlineNo}{line}" +
                               $"Error Message: {errormsg}{line}Exception Type: {extype}{line}" +
                               $"Error Location : {errorLocation}{line}Error Page Url: {exurl}{line}";

                sw.WriteLine($"----------- {foldername}##{controllername}##{methodname} {DateTime.Now} -----------------");
                sw.WriteLine(line);
                sw.WriteLine(error);
                sw.WriteLine("--------------------------------*End*------------------------------------------");
                sw.WriteLine(line);
            }
        }
    }
}

 
