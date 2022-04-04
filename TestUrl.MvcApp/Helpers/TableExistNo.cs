using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using TestUrls.TestResultLogic.BusinessModels;

namespace TestUrl.MvcApp.Helpers
{
    public static class TableExistNo
    {
        public static HtmlString TableLinks(this IHtmlHelper html, List<TestResultDto> links,
            Func<TestResultDto,bool> function)
        {
            var tag = new TagBuilder("table");
            tag.AddCssClass("table");
            tag.AddCssClass("table-bordered");
            //head
            var thead = new TagBuilder("thead");
            var tr = new TagBuilder("tr");
            var th = new TagBuilder("th");
            th.InnerHtml.Append("Count");
            tr.InnerHtml.AppendHtml(th);
            th = new TagBuilder("th");
            th.InnerHtml.Append("Link");
            tr.InnerHtml.AppendHtml(th);
            thead.InnerHtml.AppendHtml(tr);
            //body
            var tbody = new TagBuilder("tbody");

            foreach(var link in links.Where(function))
            {
                tr = new TagBuilder("tr");
                var tdCount = new TagBuilder("td");
                var tdLink = new TagBuilder("td");
                tdLink.InnerHtml.Append(link.Link);
                tr.InnerHtml.AppendHtml(tdCount);
                tr.InnerHtml.AppendHtml(tdLink);
                tbody.InnerHtml.AppendHtml(tr);
            }
            tag.InnerHtml.AppendHtml(thead);
            tag.InnerHtml.AppendHtml(tbody);

            var writer = new System.IO.StringWriter();
            tag.WriteTo(writer, HtmlEncoder.Default);
            return new HtmlString(writer.ToString());

        }
    }
}
