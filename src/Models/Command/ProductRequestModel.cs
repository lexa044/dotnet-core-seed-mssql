namespace DNSeed.Models.Command
{
    public class ProductRequestModel
    {
        public int Id { get; set; }
        public string SKU { get; set; }
        public string Name { get; set; }
        public string ImageUri { get; set; }
        public int Status { get; set; }
    }
}
