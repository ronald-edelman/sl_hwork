namespace Sanlam.Banking.Module.Metrics
{
    public interface IBankingMetricFactory
    {

        public void AddMetric(Metric metric);

        public void AddCounter(Counter counter);

    }

    //empty classes - demonstrating place for observability 
    public class Metric { }

    public class Counter { }
}
