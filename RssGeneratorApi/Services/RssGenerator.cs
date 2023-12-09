using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RssGenerator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RssGenerator.Services
{
    public class RssGenerator : IRssGenerator
    {
        private readonly RssGeneratorContext _context;
        private readonly ILogger<RssGenerator> _logger;

        public RssGenerator(RssGeneratorContext context, ILogger<RssGenerator> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> GenerateRssAsync(string feed)
        {
            _logger.LogInformation($"Generating rss for feed '{feed}'");

            var rssSource = await _context.RssSources.Where(x => x.Name == feed).Include(x => x.Articles).FirstOrDefaultAsync();

            if (rssSource == null)
                return null;

            var rss = new rss
            {
                channel = new rssChannel
                {
                    title = rssSource.Name,
                    link = rssSource.Url,
                    item = new List<rssChannelItem>()
                },
                version = 2.0m
            };

            foreach (var article in rssSource.Articles.OrderBy(x => x.PubDate))
            {
                rss.channel.item.Add(new rssChannelItem
                {
                    guid = new rssChannelItemGuid() { Value = article.Id.ToString() },
                    title = article.Title,
                    link = article.Link,
                    description = article.Description,
                    pubDate = article.PubDate.ToString("yyyy-MM-dd HH:mm:ss")
                });
            }

            var serializer = new XmlSerializer(typeof(rss));
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.Serialize(ms, rss);
                ms.Position = 0;
                using (var sr = new StreamReader(ms))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
