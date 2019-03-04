namespace Web.Configuration
{
    public class ElasticConnectionSettings
    {
        public string ClusterUrl { get; set; }
        public string BookIndex { get; set; }        
        public string PerformanceLogIndex { get; set; }        
        public string PerformanceLogType { get; set; }
        public int PerformanceLogMaxSize { get; set; }
      
    }
}