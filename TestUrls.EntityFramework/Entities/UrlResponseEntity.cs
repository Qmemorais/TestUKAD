namespace TestUrls.EntityFramework.Entities
{
    public class UrlResponseEntity
    {
        public int Id { get; set; }
        public string Link { get; set; }
        public int TimeOfResponse { get; set; }
        public int InfoEntityId { get; set; }
        public GeneralInfoEntity InfoEntity { get; set; }
    }
}
