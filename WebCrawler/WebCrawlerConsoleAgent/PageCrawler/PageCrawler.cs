using System.Threading.Channels;

namespace WebCrawlerConsoleAgent.PageCrawler;

public class PageCrawler
{
    private readonly PageParser _parser = new PageParser();
    
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
                HashSet<WebPage> subPages = await GetSubPagesAsync(webPage);
                foreach (var subPage in subPages)
                {
                    await channel.Writer.WriteAsync(subPage);
                }
            });

    }

    private async Task<HashSet<WebPage>> GetSubPagesAsync(WebPage sourcePage)
    {
        var res = string.Empty;
        using (var client = new HttpClient())
        {
            using (HttpResponseMessage message = client.GetAsync(new Uri(sourcePage.PageUrl)).Result)
            {
                using (HttpContent content = message.Content)
                {
                    res = content.ReadAsStringAsync().Result;
                }
            }
        }

        var subPages = _parser.Parse(res);
        return subPages;
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