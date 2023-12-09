using Microsoft.AspNetCore.Mvc;
using RssGenerator.Services;
using System;
using System.Threading.Tasks;

namespace RssGenerator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RssController : ControllerBase
    {
        private readonly IRssGenerator _rssGenerator;

        public RssController(IRssGenerator rssGenerator)
        {
            _rssGenerator = rssGenerator ?? throw new ArgumentNullException(nameof(rssGenerator));
        }

        [HttpGet("{feed}")]
        public async Task<ActionResult> Get(string feed)
        {
            var content = await _rssGenerator.GenerateRssAsync(feed);

            return new ContentResult
            {
                Content = content,
                ContentType = "application/xml",
                StatusCode = 200
            };
        }
    }
}
