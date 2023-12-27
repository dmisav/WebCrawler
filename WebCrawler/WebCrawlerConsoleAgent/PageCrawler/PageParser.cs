using HtmlAgilityPack;
namespace WebCrawlerConsoleAgent.PageCrawler;

public class PageParser
{
    public HashSet<string> Parse(string pageContent)
    {
        var subLinks = new HashSet<string>();
        var doc = new HtmlDocument();
        doc.LoadHtml(pageContent);
        var anchorNodes = doc.DocumentNode.SelectNodes("//a[@href]");
        if (anchorNodes == null)
            return subLinks;
        foreach (var node in anchorNodes)
        {
            var href = node.GetAttributeValue("href", "");
            subLinks.Add(href);
        }
        return subLinks;
    }
}