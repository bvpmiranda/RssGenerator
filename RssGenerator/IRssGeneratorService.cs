namespace RssGenerator
{
    public interface IRssGeneratorService
    {
        Task UpdateRssFeedsAsync(CancellationToken cancellationToken);
    }
}