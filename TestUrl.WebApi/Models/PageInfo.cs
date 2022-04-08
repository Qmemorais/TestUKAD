using System;

namespace TestUrl.WebApi.Models
{
    public class PageInfo
    {
        public int PageSize = 3;
        public int PageNumber { get; set; }
        public int TotalItems { get; set; }
        public int PageCount { get => (int)Math.Ceiling((decimal)TotalItems / PageSize); }
    }
}
