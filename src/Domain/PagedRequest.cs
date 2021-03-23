using System;

namespace DNSeed.Domain
{
    public class PagedRequest
    {
        public int AggregateId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string OrderBy { get; set; }
        public string Keyword { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
