using System;

namespace EasyFarm.DataSource.Metrics
{
    public interface IMetrics
    {
        IDisposable StartTimer(string metricName);        
    }
}