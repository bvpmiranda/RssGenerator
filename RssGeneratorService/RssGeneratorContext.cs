using Microsoft.EntityFrameworkCore;
using RssGeneratorService.Entities;

namespace RssGeneratorService
{
    public class RssGeneratorContext : DbContext
    {
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<RssSource> RssSources { get; set; }
        public virtual DbSet<RssSourceArticlePubDateReplace> RssSourceArticlePubDateReplaces { get; set; }

        public RssGeneratorContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Article.OnModelCreating(modelBuilder);
            RssSource.OnModelCreating(modelBuilder);
            RssSourceArticlePubDateReplace.OnModelCreating(modelBuilder);
        }
    }
}
