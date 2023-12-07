using System.Threading.Tasks;

namespace RssGeneratorApi.Services
{
    public interface IWebScrapperService
    {
        Task RefreshArticlesAsync();
    }
}