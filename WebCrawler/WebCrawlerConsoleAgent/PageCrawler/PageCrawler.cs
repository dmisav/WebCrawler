using System.Threading.Channels;

namespace WebCrawlerConsoleAgent.PageCrawler;

public class PageCrawler
{
    public async void StartCrawlAsync()
    {
        var channel = Channel.CreateUnbounded<WebPage>();
        var ctx = new CancellationTokenSource().Token;
        var crawlOptions = new ParallelOptions()
        {
            CancellationToken = ctx
        };

        await Parallel.ForEachAsync(channel.Reader.ReadAllAsync(),
            ctx, 
            async (webPage, ctx) =>
            {
                WebPage page = await GetPageUrlsAsync(webPage);
                foreach (var subPage in page.subPages)
                {
                    channel.Writer.WriteAsync(subPage);
                }
            });

    }
}

/*
var channel = Channel.CreateUnbounded<CrawlerPage>();
channel.Writer.TryWrite(new CrawlerPage(sourceLink));

var cts = new CancellationTokenSource();
var options = new ParallelOptions()
{
    MaxDegreeOfParallelism = 10,
    CancellationToken = cts.Token
};

await Parallel.ForEachAsync(channel.Reader.ReadAllAsync(), async (page, ct) =>
{
    CrawlerPage[] subpages = await GetPagesAsync(page);
    foreach (var subpage in subpages) channel.Writer.TryWrite(subpage);
});*/