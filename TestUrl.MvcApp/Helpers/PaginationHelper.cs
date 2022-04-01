using System;
using System.Text;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using TestUrl.MvcApp.Models;

namespace TestUrl.MvcApp.Helpers
{
    public static class PaginationHelper
    {
        public static HtmlString PageLinks(this IHtmlHelper html, PageInfo pageInfo, Func<int, string> pageUrl)
        {
            var result = new StringBuilder();

            for (int i = 1; i <= pageInfo.PageCount; i++)
            {
                var tag = new System.Web.Mvc.TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToString();

                if (i == pageInfo.PageNumber)
                {
                    tag.AddCssClass("viewedPage");
                }

                result.Append(tag.ToString());
            }
            return new HtmlString(result.ToString());
        }
    }
}
