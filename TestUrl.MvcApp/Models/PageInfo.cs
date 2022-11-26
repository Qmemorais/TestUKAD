using System;

namespace TestUrl.MvcApp.Models
{
    public class PageInfo
    {
        public int PageSize = 3;
        public int PageNumber { get; set; }
        public int TotalItems { get; set; }
        public int PageCount { get => (int)Math.Ceiling((decimal)TotalItems / PageSize); }
    }
}
