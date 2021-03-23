using System.Collections.Generic;

namespace DNSeed.Models.Query
{
    public class PagedResponseModel<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
