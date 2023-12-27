using HtmlAgilityPack;
using WebCrawlerConsoleAgent.PageCrawler;

[TestFixture]
public class HtmlParserTests
{
    private PageParser sut = new PageParser();
    
    [Test]
    public void Parse_ShouldExtractSubLinksFromHtml()
    {
        // Arrange
        var htmlParser = sut;
        var htmlContent = @"
            <html>
                <body>
                    <a href='https://example.com/page1'>Page 1</a>
                    <a href='https://example.com/page2'>Page 2</a>
                    <a href='https://example.com/page3'>Page 3</a>
                </body>
            </html>";

        // Act
        var result = htmlParser.Parse(htmlContent);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(3, result.Count);
        Assert.That(result, Contains.Item("https://example.com/page1"));
        Assert.That(result, Contains.Item("https://example.com/page2"));
        Assert.That(result, Contains.Item("https://example.com/page3"));
    }

    [Test]
    public void Parse_ShouldHandleEmptyHtmlContent()
    {
        // Arrange
        var htmlParser = sut;
        var emptyHtmlContent = string.Empty;

        // Act
        var result = htmlParser.Parse(emptyHtmlContent);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsEmpty(result);
    }
}
