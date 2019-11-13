using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace NorthwindWeb.HtmlHelpers
{
    public static class NorthwindImageLinkHelper
    {
        private static readonly string _imageWidth = "100";
        private static readonly string _imageHeight = "100";

        public static HtmlString NorthwindImageLink(this IHtmlHelper html, string imageId)
        {
            var urlHelper = new UrlHelper(html.ViewContext);
            var url = urlHelper.RouteUrl("image", new { id = imageId });

            var img = new TagBuilder("img");
            img.Attributes.Add("width", _imageWidth);
            img.Attributes.Add("height", _imageHeight);
            img.Attributes.Add("src", url);

            var a = new TagBuilder("a");
            a.Attributes.Add("href", url);
            a.InnerHtml.AppendHtml(img);

            var writer = new StringWriter();
            a.WriteTo(writer, HtmlEncoder.Default);

            return new HtmlString(writer.ToString());
        }
    }
}
