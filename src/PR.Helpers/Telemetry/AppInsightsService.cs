using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PR.Helpers.Contract;
using PR.Helpers.Models;

namespace PR.Helpers.Telemetry
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.DataContracts;
    
    namespace Xamling.Azure.Logger
    {
        public class LogService : ILogService
        {
            private readonly TelemetryClient _telemetry;
            private readonly AppInsightsSettings _settings;
            public LogService(IOptions<AppInsightsSettings> settings)
            {
                
                _settings = settings.Value;

                Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration.Active.InstrumentationKey =
                    _settings.InstrumentationKey;

                _telemetry = new TelemetryClient(new TelemetryConfiguration
                {
                    InstrumentationKey = _settings.InstrumentationKey
                });
            }

            public void TrackEvent(string eventName, IDictionary<string, string> properties = null,
                IDictionary<string, double> metrics = null)
            {
                _telemetry.TrackEvent(eventName, properties, metrics);
            }

            public void TrackEvent(string eventName, string key, string value)
            {
                var dict = new Dictionary<string, string> {{key, value}};
                _telemetry.TrackEvent(eventName, dict, null);
            }

            public void TrackEventObject<T>(string eventName, string key, T value)
            {
                var ser = JsonConvert.SerializeObject(value);
                var dict = new Dictionary<string, string> {{key, ser}};
                _telemetry.TrackEvent(eventName, dict, null);
            }

            public void TrackException(Exception exception, IDictionary<string, string> properties = null,
                IDictionary<string, double> metrics = null)
            {
                _telemetry.TrackException(exception, properties, metrics);
            }

            public void TrackException(Exception exception, string message)
            {
                var dict = new Dictionary<string, string> { { "message", message } };
                _telemetry.TrackException(exception, dict);
            }

            public void TrackMetric(string name, double value, IDictionary<string, string> properties = null)
            {
                _telemetry.TrackMetric(name, value, properties);
            }

            public void TrackPageView(string name)
            {
                _telemetry.TrackPageView(name);
            }

            public void TrackRequest(string name, DateTimeOffset timestamp, TimeSpan duration, string responseCode,
                bool success)
            {
                _telemetry.TrackRequest(name, timestamp, duration, responseCode, success);
            }

            public void TrackTrace(string message)
            {
                _telemetry.TrackTrace(message);
            }

            public void TrackTrace(string message, IDictionary<string, string> properties)
            {
                _telemetry.TrackTrace(message, properties);
            }

            public void TrackTrace(string message, string key, string value)
            {
                var dict = new Dictionary<string, string> { { key, value } };
                _telemetry.TrackTrace(message, dict);
            }
            
        }
    }

}
