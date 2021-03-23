namespace DNSeed.Models.Query
{
    public class ProductResponseModel
    {
        public int Id { get; set; }
        public string SKU { get; set; }
        public string Name { get; set; }
        public string ImageUri { get; set; }
        public int Status { get; set; }
        public long CreatedDate { get; set; }
        public long UpdatedDate { get; set; }
    }
}
