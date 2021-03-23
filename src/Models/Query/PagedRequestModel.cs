namespace DNSeed.Models.Query
{
    public class PagedRequestModel
    {
        public int AggregateId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string OrderBy { get; set; }
        public string Keyword { get; set; }
        public long From { get; set; }
        public long To { get; set; }
    }
}
