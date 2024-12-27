namespace CoreAPI_V3.Interface
{
    public interface ILoggerService
    {
        void LogError(string foldername, string controllername, string methodname, Exception ex);
    }
}
