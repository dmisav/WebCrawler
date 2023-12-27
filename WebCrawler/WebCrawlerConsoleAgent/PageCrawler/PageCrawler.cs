using System.Threading.Channels;

namespace WebCrawlerConsoleAgent.PageCrawler;

public class PageCrawler
{
    private readonly PageParser _parser = new PageParser();
    string userAgent = "ConsoleAppUserAgent/1.0";
    public async Task StartCrawlAsync(string starturl, CancellationToken ctx)
    {
        var channel = Channel.CreateUnbounded<string>();
        var crawlOptions = new ParallelOptions()
        {
            MaxDegreeOfParallelism = 100,
            CancellationToken = ctx
        };
        await channel.Writer.WriteAsync(starturl);

        await Parallel.ForEachAsync(channel.Reader.ReadAllAsync(),
            ctx, 
            async (webPage, ctx) =>
            {
                HashSet<string> subPages = await GetSubPagesAsync(new Uri(webPage));
                foreach (var subPage in subPages)
                {
                    if (Uri.TryCreate(subPage, UriKind.Absolute, out _))
                    {
                        if (new Uri(subPage).Scheme != "file")
                            await channel.Writer.WriteAsync(subPage);
                    }
                }
            });
    }

    private async Task<HashSet<string>> GetSubPagesAsync(Uri sourcePageUrl)
    {
        var res = string.Empty;
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("User-Agent", userAgent);
            using (HttpResponseMessage message = client.GetAsync(sourcePageUrl).Result)
            {
                using (HttpContent content = message.Content)
                {
                    res = content.ReadAsStringAsync().Result;
                }
            }
        }
        Console.WriteLine(sourcePageUrl.AbsoluteUri);
        var subPages = _parser.Parse(res);
        return subPages;
    }
}