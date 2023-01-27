using RssGenerator.Models;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Serialization;

namespace RssGenerator
{
    public class RssGeneratorService : IRssGeneratorService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly Infrastructure.ILogger _logger;

        public RssGeneratorService(IConfiguration configuration, Infrastructure.ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;

            _httpClient = new HttpClient();
        }

        public async Task UpdateRssFeedsAsync(CancellationToken cancellationToken)
        {
            var sources = new List<RssSource>();
            ConfigurationBinder.Bind(_configuration.GetSection("Sources"), sources);

            foreach (var source in sources)
            {
                _logger.Info("Processing {name}: {time}", source.Name, DateTimeOffset.Now);

                var request = new HttpRequestMessage(HttpMethod.Get, source.Url);
                request.Headers.Add("User-Agent", "RssGenerator");

                HttpResponseMessage response;

                try
                {
                    response = _httpClient.Send(request, cancellationToken);

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.Error($"The respose for feed '{source.Name}' was {response.StatusCode}");
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error($"There was an error trying to update feed '{source.Name}'", ex);
                    
                    continue;
                }

                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(await response.Content.ReadAsStringAsync());


                var filename = Path.Combine(_configuration.GetValue<string>("RssFeedsLocation"), $"{source.Name.Replace(" ", "")}.xml");
                var serializer = new XmlSerializer(typeof(rss));

                rss rss;

                if (!File.Exists(filename))
                {
                    rss = new rss
                    {
                        channel = new rssChannel
                        {
                            title = source.Name,
                            link = source.Url,
                            item = new List<rssChannelItem>()
                        },
                        version = 2.0m
                    };

                    using (FileStream fs = new FileStream(filename, FileMode.Create))
                    {
                        serializer.Serialize(fs, rss);
                    }
                }
                else
                {
                    using (FileStream fs = new FileStream(filename, FileMode.Open))
                    {
                        rss = (rss)serializer.Deserialize(fs);
                    }
                }

                var nodes = doc.DocumentNode.SelectNodes(source.ArticlesXPath);
                foreach (var node in nodes)
                {
                    var title = node.SelectSingleNode($"{node.XPath}{source.ArticleTitleXPath}").InnerText;
                    var description = node.SelectSingleNode($"{node.XPath}{source.ArticleDescriptionXPath}");

                    var link = node.SelectSingleNode($"{node.XPath}{source.ArticleLinkXPath}").Attributes["href"].Value;
                    if (!string.IsNullOrWhiteSpace(source.ArticleLinkPrefix))
                        link = $"{source.ArticleLinkPrefix}/{link.TrimStart('/')}";

                    var pubDateString = node.SelectSingleNode($"{node.XPath}{source.ArticlePubDateXPath}").InnerText.Trim();
                    DateTime? pubDate = null;

                    if (!string.IsNullOrWhiteSpace(source.ArticlePubDateRegex))
                    {
                        var pubDateMatch = Regex.Match(pubDateString, source.ArticlePubDateRegex);

                        if (pubDateMatch.Success)
                            pubDateString = pubDateMatch.Value;
                    }

                    if (source.ArticlePubDateReplace?.Any() ?? false)
                    {
                        foreach (var key in source.ArticlePubDateReplace.Keys)
                        {
                            pubDateString = pubDateString.Replace(key, source.ArticlePubDateReplace[key]);
                        }
                    }

                    if (DateTime.TryParseExact(pubDateString, source.ArticlePubDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                        pubDate = date;

                    var item = new rssChannelItem
                    {
                        guid = new rssChannelItemGuid() { Value = Guid.NewGuid().ToString() },
                        title = HttpUtility.HtmlDecode(title),
                        link = link,
                        description = HttpUtility.HtmlDecode(description.InnerText),
                        pubDate = pubDate?.ToString("yyyy-MM-dd HH:mm:ss")
                    };

                    if (rss.channel.item.Any(x => x.description == item.description))
                        continue;

                    rss.channel.item.Add(item);
                }

                using (FileStream fs = new FileStream(filename, FileMode.Create))
                {
                    serializer.Serialize(fs, rss);
                }
            }
        }
    }
}
