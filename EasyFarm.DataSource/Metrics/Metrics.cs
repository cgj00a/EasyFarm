using System;
using System.Diagnostics;

namespace EasyFarm.DataSource.Metrics
{
    public class Metrics : IMetrics, IDisposable
    {
        private readonly IMetricsRepository _metricsRepository;
        private Stopwatch _stopwatch;
        private string _metricName;

        public Metrics(IMetricsRepository metricsRepository)
        {
            _metricsRepository = metricsRepository;
        }

        public IDisposable StartTimer(string metricName)
        {
            _metricName = metricName;
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
            return this;
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            _metricsRepository.SetTimeMetric(_metricName, _stopwatch.ElapsedTicks);
        }
    }   
}