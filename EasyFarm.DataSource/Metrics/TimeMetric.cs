using System;

namespace EasyFarm.DataSource.Metrics
{
    public class TimeMetric
    {
        public string Name { get; set; }
        public long Ticks { get; set; }
        public TimeSpan TimeSpan => TimeSpan.FromTicks(Ticks);
    }
}