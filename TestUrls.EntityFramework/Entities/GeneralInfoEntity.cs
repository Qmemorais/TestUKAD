using System;
using System.Collections.Generic;

namespace TestUrls.EntityFramework.Entities
{
    public class GeneralInfoEntity
    {
        public int Id { get; set; }
        public string Link { get; set; }
        public DateTime CreateAt { get; set; }
        public List<UrlEntity> UrlEntities { get;set;}
        public List<UrlResponseEntity> UrlResponseEntities { get; set; }
    }
}
