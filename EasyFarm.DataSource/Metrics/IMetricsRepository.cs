namespace EasyFarm.DataSource.Metrics
{
    public interface IMetricsRepository
    {
        TimeMetric GetTimeMetric(string metricName);
        void SetTimeMetric(string metricName, long elapsedTicks);
    }
}