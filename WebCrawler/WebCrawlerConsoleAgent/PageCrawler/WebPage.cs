namespace WebCrawlerConsoleAgent.PageCrawler;

public class WebPage(string url)
{
    private readonly string _url = url;
    public List<WebPage> subPages;
}