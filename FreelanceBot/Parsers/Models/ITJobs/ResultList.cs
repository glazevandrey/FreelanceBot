using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System;

namespace FreelanceBot.Parsers.Models.ITJobs
{
    public class ResultList
    {
        public int id { get; set; }
        public Company company { get; set; }
        public int companyId { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public object @ref { get; set; }
        public bool allowRemote { get; set; }
        public int? wage { get; set; }
        public List<Location> locations { get; set; }
        public string publishedAt { get; set; }
        public string updatedAt { get; set; }
        public string slug { get; set; }
        public List<Type> types { get; set; }
    }
}
