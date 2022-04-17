namespace TestUrls.EntityFramework.Entities
{
    public class TestResult
    {
        public int Id { get; set; }
        public string Link { get; set; }
        public bool IsSitemap { get; set; }
        public bool IsWeb { get; set; }
        public int TimeOfResponse { get; set; }

        public int TestId { get; set; }
        public Test Test { get; set; }
    }
}
