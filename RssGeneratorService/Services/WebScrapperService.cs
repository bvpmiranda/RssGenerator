using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RssGenerator;
using RssGenerator.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace RssGenerator.Services
{
    public class WebScrapperService : IWebScrapperService
    {
        private readonly RssGeneratorContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<WebScrapperService> _logger;

        public WebScrapperService(RssGeneratorContext context, IHttpClientFactory httpClientFactory, ILogger<WebScrapperService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _httpClientFactory = httpClientFactory;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task RefreshArticlesAsync()
        {
            _logger.LogInformation("Refreshing all feeds");

            var feeds = await _context.RssSources.Where(x => x.Enabled).Select(x => x.Name).ToListAsync();

            foreach(var feed in feeds)
            {
                await RefreshArticles(feed);
            }
        }

        private async Task RefreshArticles(string feed)
        {
            _logger.LogInformation($"Refreshing feed '{feed}'");

            try
            {
                var source = await _context.RssSources.Where(x => x.Name == feed && x.Enabled).Include(x => x.ArticlePubDateReplace).FirstOrDefaultAsync();

                if (source == null)
                    return;

                var httpClient = _httpClientFactory.CreateClient();

                var articles = await GetArticles(source);

                if (articles == null)
                    return;

                var titles = articles.Select(x => x.Title).ToList();
                var titlesThatAlreadyExists = await _context.Articles.Where(x => x.RssSourceId == source.Id && titles.Contains(x.Title)).Select(x => x.Title).ToListAsync();
                var articlesToRemove = articles.Where(x => titlesThatAlreadyExists.Contains(x.Title)).ToList();
                articles = articles.Except(articlesToRemove).ToList();

                if (articles.Any())
                {
                    _context.AddRange(articles);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private async Task<List<Article>> GetArticles(RssSource source)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, source.Url);
                request.Headers.Add("User-Agent", "RssGenerator");

                HttpResponseMessage response;

                var httpClient = _httpClientFactory.CreateClient();

                try
                {
                    _logger.LogInformation($"Making request to '{source.Url}'");

                    response = httpClient.Send(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError($"The response for feed '{source.Name}' was {response.StatusCode}");
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"There was an error trying to update feed '{source.Name}'");

                    return null;
                }
                var responseContent = await response.Content.ReadAsStringAsync();

                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(responseContent);

                var nodes = doc.DocumentNode.SelectNodes(source.ArticlesXPath);

                var articles = new List<Article>();

                foreach (var node in nodes)
                {
                    try
                    {
                        var title = HttpUtility.HtmlDecode(GetArticleTitle(source, node));
                        var description = HttpUtility.HtmlDecode(GetArticleDescription(source, node).InnerText);
                        var link = GetArticleLink(source, node);
                        var pubDate = GetArticlePubDate(source, node);

                        articles.Add(new Article
                        {
                            Id = Guid.NewGuid(),
                            RssSourceId = source.Id,
                            Title = title,
                            Description = description,
                            Link = link,
                            PubDate = new DateTime(pubDate.Year, pubDate.Month, pubDate.Day, pubDate.Hour, pubDate.Minute, pubDate.Second, pubDate.Millisecond)
                        });
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"{node.OuterHtml}\r\n\r\n{ex.Message}" );
                    }
                }

                return articles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return null;
            }
        }

        private static string GetArticleTitle(RssSource source, HtmlNode article)
        {

            return article.SelectSingleNode($"{article.XPath}{source.ArticleTitleXPath}").InnerText;
        }

        private static HtmlNode GetArticleDescription(RssSource source, HtmlNode article)
        {
            return article.SelectSingleNode($"{article.XPath}{source.ArticleDescriptionXPath}");
        }

        private static string GetArticleLink(RssSource source, HtmlNode article)
        {
            var link = article.SelectSingleNode($"{article.XPath}{source.ArticleLinkXPath}").Attributes["href"].Value;
            if (!string.IsNullOrWhiteSpace(source.ArticleLinkPrefix))
                link = $"{source.ArticleLinkPrefix}/{link.TrimStart('/')}";
            return link;
        }

        private static DateTime GetArticlePubDate(RssSource source, HtmlNode article)
        {
            var pubDateString = article.SelectSingleNode($"{article.XPath}{source.ArticlePubDateXPath}").InnerText.Trim();
            DateTime? pubDate = null;

            if (source.ArticlePubDateHasAgoPattern && Regex.Match(pubDateString, "[\\d]+ (hour|min|minute|second)(|s) ago", RegexOptions.IgnoreCase).Success)
                return GetArticlePubDateWithAgoPattern(source, pubDateString);

            if (!string.IsNullOrWhiteSpace(source.ArticlePubDateRegex))
                pubDateString = GetArticlePubDateWithRegex(source, pubDateString);

            if (source.ArticlePubDateReplace?.Any() ?? false)
                pubDateString = GetArticlePubDateWithReplaces(source, pubDateString);

            if (DateTime.TryParseExact(pubDateString, source.ArticlePubDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                pubDate = date;

            if (!pubDate.HasValue)
                throw new Exception("Invalid date");

            if (source.ArticlePubDateTimeZone == "Pacific Standard Time")
                return pubDate.Value;

            return ConvertToTimeZone(pubDate.Value, source.ArticlePubDateTimeZone);
        }

        private static DateTime GetArticlePubDateWithAgoPattern(RssSource source, string pubDateString)
        {
            DateTime pubDate;

            if (Regex.Match(pubDateString, "[\\d]+ (hour)(|s) ago", RegexOptions.IgnoreCase).Success)
            {
                var hours = int.Parse(Regex.Replace(pubDateString, " (hour)(|s) ago", "", RegexOptions.IgnoreCase).Trim());
                pubDate = DateTime.UtcNow.AddHours(-hours);
            }
            else if (Regex.Match(pubDateString, "[\\d]+ min(|ute)(|s) ago", RegexOptions.IgnoreCase).Success)
            {
                var minutes = int.Parse(Regex.Replace(pubDateString, " min(|ute)(|s) ago", "", RegexOptions.IgnoreCase).Trim());
                pubDate = DateTime.UtcNow.AddMinutes(-minutes);
            }
            else
            {
                var seconds = int.Parse(Regex.Replace(pubDateString, " (second)(|s) ago", "", RegexOptions.IgnoreCase).Trim());
                pubDate = DateTime.UtcNow.AddSeconds(-seconds);
            }

            if (source.ArticlePubDateTimeZone == "Pacific Standard Time")
                return pubDate;

            return ConvertToTimeZone(pubDate, source.ArticlePubDateTimeZone);
        }

        private static string GetArticlePubDateWithRegex(RssSource source, string pubDateString)
        {
            var pubDateMatch = Regex.Match(pubDateString, source.ArticlePubDateRegex);

            if (pubDateMatch.Success)
                pubDateString = pubDateMatch.Value;

            return pubDateString;
        }

        private static string GetArticlePubDateWithReplaces(RssSource source, string pubDateString)
        {
            foreach (var replaceRule in source.ArticlePubDateReplace)
            {
                pubDateString = pubDateString.Replace(replaceRule.Value, replaceRule.Replacement);
            }

            return pubDateString;
        }

        static DateTime ConvertToTimeZone(DateTime utcDateTime, string timeZoneId)
        {
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

            DateTime convertedDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, timeZoneInfo);

            return convertedDateTime;
        }
    }
}
