using System.Threading.Tasks;

namespace RssGenerator.Services
{
    public interface IWebScrapperService
    {
        Task RefreshArticlesAsync();
    }
}