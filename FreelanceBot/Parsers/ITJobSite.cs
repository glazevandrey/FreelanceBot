using FreelanceBot.Parsers.Models.ITJobs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace FreelanceBot.Parsers
{
    public class ITJobSite
    {
        public string ApiKey { get; set; } = "bb5d204833115417b270e4e742155518";
        public Dictionary<string, List<ResultList>> ParseNewJobs()
        {
            Dictionary<string, List<ResultList>> resultITJobs = new Dictionary<string, List<ResultList>>();

            for (int i = 0; i < Program.config.Specialization.Count; i++)
            {
                var f = GetSearch(Program.config.Specialization[i]);
                int page = 1;
                for (; ;)
                {
                    var f2 = GetSearchPage(Program.config.Specialization[i], (page + 1).ToString());
                    if (f2.results == null)
                    {
                        break;
                    }

                    f.results.AddRange(f2.results);
                    page += 1;
                }
                if(f.results != null && f.results.Count > 0)
                {
                    resultITJobs.Add(Program.config.Specialization[i], f.results);
                }
            }


            return resultITJobs;
        }
        private GetListResponse GetList()
        {
            var raw_data = Requests.HttpGetListITJobs("https://api.itjobs.pt/job/list.json", ApiKey);
            return JsonConvert.DeserializeObject<GetListResponse>(raw_data);

        }
        private GetListResponse GetSearch(string q)
        {
            var raw_data = Requests.HttpGetSearchITJobs("https://api.itjobs.pt/job/search.json", ApiKey, q, null);
            return JsonConvert.DeserializeObject<GetListResponse>(raw_data);
        }
        private GetListResponse GetSearchPage(string q, string p)
        {
            var raw_data = Requests.HttpGetSearchITJobs("https://api.itjobs.pt/job/search.json", ApiKey, q, p);
            return JsonConvert.DeserializeObject<GetListResponse>(raw_data);
        }
    }
}
