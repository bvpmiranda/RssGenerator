using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace RssGenerator.Entities
{
    public class RssSource
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Url { get; set; }
        public string ArticlesXPath { get; set; }
        public string ArticleTitleXPath { get; set; }
        public string ArticleLinkXPath { get; set; }
        public string ArticleLinkPrefix { get; set; }
        public string ArticleDescriptionXPath { get; set; }
        public string ArticlePubDateXPath { get; set; }
        public bool ArticlePubDateHasAgoPattern { get; set; }
        public string ArticlePubDateTimeZone { get; set; }
        public string ArticlePubDateFormat { get; set; }
        public string ArticlePubDateRegex { get; set; }
        public bool Enabled { get; set; }

        public ICollection<Article> Articles { get; set; }
        public ICollection<RssSourceArticlePubDateReplace> ArticlePubDateReplace { get; set; }

        internal static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RssSource>(entity =>
            {
                entity.ToTable("rsssources");

                entity.Property(e => e.Id)
                    .HasColumnName("rsssourceid")
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasColumnName(nameof(Name).ToLower())
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Url)
                    .HasColumnName(nameof(Url).ToLower())
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ArticlesXPath)
                    .HasColumnName(nameof(ArticlesXPath).ToLower())
                    .IsRequired()
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.ArticleTitleXPath)
                    .HasColumnName(nameof(ArticleTitleXPath).ToLower())
                    .IsRequired()
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.ArticleLinkXPath)
                    .HasColumnName(nameof(ArticleLinkXPath).ToLower())
                    .IsRequired()
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.ArticleLinkPrefix)
                    .HasColumnName(nameof(ArticleLinkPrefix).ToLower())
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.ArticleDescriptionXPath)
                    .HasColumnName(nameof(ArticleDescriptionXPath).ToLower())
                    .IsRequired()
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.ArticlePubDateXPath)
                    .HasColumnName(nameof(ArticlePubDateXPath).ToLower())
                    .IsRequired()
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.ArticlePubDateHasAgoPattern)
                    .HasColumnName(nameof(ArticlePubDateHasAgoPattern).ToLower())
                    .IsRequired();

                entity.Property(e => e.ArticlePubDateTimeZone)
                    .HasColumnName(nameof(ArticlePubDateTimeZone).ToLower())
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ArticlePubDateFormat)
                    .HasColumnName(nameof(ArticlePubDateFormat).ToLower())
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ArticlePubDateRegex)
                    .HasColumnName(nameof(ArticlePubDateRegex).ToLower())
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.Enabled)
                    .HasColumnName(nameof(Enabled).ToLower())
                    .IsRequired();
            });
        }
    }
}
