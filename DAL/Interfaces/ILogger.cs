using System;

namespace DAL.Interfaces
{
    public interface ILogger
    {
        void LogInfo(string info);
        void LogError(Exception ex);
    }
}
