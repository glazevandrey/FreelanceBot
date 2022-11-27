using System.Collections.Generic;

namespace FreelanceBot.Parsers.Models.ITJobs
{
    public class GetListResponse
    {
        public int total { get; set; }
        public int page { get; set; }
        public int limit { get; set; }
        public List<ResultList> results { get; set; }
    }
}
