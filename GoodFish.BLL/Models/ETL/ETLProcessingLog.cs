using System.Text.Json.Serialization;

namespace GoodFish.BLL.Models.ETL
{
    public class ETLProcessingLog
    {
        [JsonPropertyName("timestamp")]
        public DateTime TimeStamp { get; set; }
        public required ETLProcessingStatistics[] Statistics { get; set; }
        public required ETLProcessingSummary Summary { get; set; }


        public class ETLProcessingStatistics
        {
            public required string EntityName { get; set; }
            public int TotalRecords { get; set; }
            public int ValidRecords { get; set; }
            public int ErrorRecords { get; set; }
            public int CreatedRecords { get; set; }
            public string[] Errors { get; set; } = [];
        }

        public class ETLProcessingSummary
        {
            public int TotalRecords { get; set; }
            public int TotalValid { get; set; }
            public int TotalError { get; set; }
            public int TotalCreated { get; set; }
        }
    }
}
