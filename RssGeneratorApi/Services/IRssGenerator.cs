using System.Threading.Tasks;

namespace RssGenerator.Services
{
    public interface IRssGenerator
    {
        Task<string> GenerateRssAsync(string feed);
    }
}