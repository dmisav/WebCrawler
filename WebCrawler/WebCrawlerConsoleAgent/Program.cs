
using WebCrawlerConsoleAgent.PageCrawler;

internal class Program
{
    public static void Main(string[] args)
    {
        var crawler = new PageCrawler();
        var ctx = new CancellationTokenSource().Token;
        Task.Factory.StartNew(()=>crawler.StartCrawlAsync("https://en.wikipedia.org/wiki/Constantinople",ctx));
        Console.WriteLine("Crawling in progress");
        Console.ReadLine();
    }
}