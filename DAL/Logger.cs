using DAL.Interfaces;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using System;

namespace DAL
{
    public class Logger : ILogger
    {
        private readonly TelemetryClient _telemetryClient;

        public Logger(string instrumentationKey)
        {
            _telemetryClient = new TelemetryClient(new TelemetryConfiguration { InstrumentationKey = instrumentationKey });
        }

        public void LogError(Exception ex)
        {
            _telemetryClient.TrackException(ex);
        }

        public void LogInfo(string info)
        {
            _telemetryClient.TrackEvent(info);
        }
    }
}
