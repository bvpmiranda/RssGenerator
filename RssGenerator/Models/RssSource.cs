using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssGenerator.Models
{
    public class RssSource
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string ArticlesXPath { get; set; }
        public string ArticleTitleXPath { get; set; }
        public string ArticleLinkXPath { get; set; }
        public string ArticleLinkPrefix { get; set; }
        public string ArticleDescriptionXPath { get; set; }
        public string ArticlePubDateXPath { get; set; }
        public string ArticlePubDateRegex { get; set; }
        public string ArticlePubDateFormat { get; set; }
        public Dictionary<string, string> ArticlePubDateReplace { get; set; }
    }
}
