using Microsoft.AspNetCore.Mvc.Rendering;

namespace FarAwayClient.Tools
{
    public static class HtmlExtensions
    {
        public static string IsActive(this IHtmlHelper htmlHelper, string page)
        {
            var currentPage = htmlHelper.ViewContext.RouteData.Values["page"]?.ToString();
            return string.Equals(currentPage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
