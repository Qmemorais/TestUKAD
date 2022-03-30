using System;
using System.Collections.Generic;

namespace TestUrls.EntityFramework.Entities
{
    public class Test
    {
        public int Id { get; set; }
        public string Link { get; set; }
        public DateTime CreateAt { get; set; }
        public ICollection<TestResult> UrlWithResponseEntities { get;set;}
    }
}
