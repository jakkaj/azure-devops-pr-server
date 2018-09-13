using System;
using System.Collections.Generic;

namespace PR.Helpers.Contract
{
    public interface ILogService
    {
        void TrackEvent(string eventName, IDictionary<string, string> properties = null,
            IDictionary<string, double> metrics = null);

        void TrackException(Exception exception, IDictionary<string, string> properties = null,
            IDictionary<string, double> metrics = null);

        void TrackMetric(string name, double value, IDictionary<string, string> properties = null);
        void TrackPageView(string name);

        void TrackRequest(string name, DateTimeOffset timestamp, TimeSpan duration, string responseCode,
            bool success);

        void TrackTrace(string message);
        void TrackTrace(string message, IDictionary<string, string> properties);
        void TrackEvent(string eventName, string key, string value);
        void TrackEventObject<T>(string eventName, string key, T value);
        void TrackTrace(string message, string key, string value);
        void TrackException(Exception exception, string message);
    }
}