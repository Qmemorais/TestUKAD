using System;

namespace TestUrl.MvcApp.Models
{
    public class PageInfo
    {
        public int PageSize = 5;
        public int PageNumber { get; set; }
        public int TotalItems { get; set; }
        public int PageCount { get { return (int)Math.Ceiling((decimal)TotalItems / PageSize); } }
    }
}
