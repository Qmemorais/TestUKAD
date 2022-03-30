namespace TestUrls.EntityFramework.Entities
{
    public class UrlWithResponse
    {
        public int Id { get; set; }
        public string Link { get; set; }
        public bool IsSitemap { get; set; }
        public bool IsWeb { get; set; }
        public int TimeOfResponse { get; set; }
        public int TestEntityId { get; set; }
        public SiteTestEntity TestEntity { get; set; }
    }
}
