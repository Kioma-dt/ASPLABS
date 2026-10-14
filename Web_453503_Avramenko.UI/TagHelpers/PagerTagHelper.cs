using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Web_453503_Avramenko.UI.TagHelpers;

[HtmlTargetElement("pager")]
public class PagerTagHelper
    : TagHelper
{
    private readonly LinkGenerator _linkGenerator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public string Category { get; set; }  = string.Empty;
    public bool  Admin { get; set; }  = false;

    public PagerTagHelper(
        LinkGenerator linkGenerator,
        IHttpContextAccessor httpContextAccessor)
    {
        _linkGenerator = linkGenerator;
        _httpContextAccessor = httpContextAccessor;
    }
    

    public override void Process(
        TagHelperContext context, 
        TagHelperOutput output)
    {
        var httpContext = _httpContextAccessor.HttpContext
                          ?? throw new InvalidOperationException(
                              "HttpContext is unavailable.");
        
        var prev = CurrentPage == 1
            ? 1
            : CurrentPage - 1;
        var next = CurrentPage == TotalPages
            ? TotalPages
            : CurrentPage + 1;
        
        var nav = new TagBuilder("nav");
        nav.AddCssClass("mt-4");
        
        var ul = new TagBuilder("ul");
        ul.AddCssClass("pagination justify-content-center");
        
        var liPrev = new TagBuilder("li");
        liPrev.AddCssClass("page-item");
        
        var aPrev = new TagBuilder("a");
        aPrev.AddCssClass("page-link");
        string? urlPrev = null;
        
        if (!Admin)
        {
            urlPrev = _linkGenerator.GetPathByRouteValues(
                httpContext,
                routeName: "catalog",
                values: new
                {
                    species = Category,
                    page = prev
                });
            
        }
        else
        {
            urlPrev = _linkGenerator.GetPathByPage(
                httpContext,
                page: "/Index",
                values: new
                {
                    area= "Admin",
                    pageNumber = prev
                });
        }
        aPrev.Attributes["href"] = urlPrev ?? "#";
        aPrev.InnerHtml.Append("Prev");
        liPrev.InnerHtml.AppendHtml(aPrev);
        
        ul.InnerHtml.AppendHtml(liPrev);

        for (int pageNumber = 1; pageNumber <= TotalPages; pageNumber++)
        {
            var liCurrent = new TagBuilder("li");
            liCurrent.AddCssClass("page-item");
            if (CurrentPage == pageNumber)
            {
                liCurrent.AddCssClass("active");
            }
            var aCurrent = new TagBuilder("a");
            aCurrent.AddCssClass("page-link");
            string? urlCurrent = null;

            if (!Admin)
            {
                urlCurrent = _linkGenerator.GetPathByRouteValues(
                    httpContext,
                    routeName: "catalog",
                    values: new
                    {
                        species = Category,
                        page = pageNumber
                    });
            }
            else
            {
                urlCurrent = _linkGenerator.GetPathByPage(
                    httpContext,
                    page: "/Index",
                    values: new
                    {
                        area= "Admin",
                        pageNumber = pageNumber
                    });
            }
            aCurrent.Attributes["href"] = urlCurrent ?? "#";
            aCurrent.InnerHtml.AppendHtml(pageNumber.ToString());
            liCurrent.InnerHtml.AppendHtml(aCurrent);
            ul.InnerHtml.AppendHtml(liCurrent);
        }
        
        var liNext = new TagBuilder("li");
        liNext.AddCssClass("page-item");
        
        var aNext = new TagBuilder("a");
        aNext.AddCssClass("page-link");
        string? urlNext = null;
        
        if (!Admin)
        {
            urlNext = _linkGenerator.GetPathByRouteValues(
                httpContext,
                routeName: "catalog",
                values: new
                {
                    species = Category,
                    page = next
                });
            
        }
        else
        {
            urlNext = _linkGenerator.GetPathByPage(
                httpContext,
                page: "/Index",
                values: new
                {
                    area= "Admin",
                    pageNumber = next
                });
        }
        aNext.Attributes["href"] = urlNext ?? "#";
        aNext.InnerHtml.Append("Next");
        liNext.InnerHtml.AppendHtml(aNext);
        
        ul.InnerHtml.AppendHtml(liNext);
        
        nav.InnerHtml.AppendHtml(ul);
        
        output.TagName = null;
        output.Content.SetHtmlContent(nav);
    }
}