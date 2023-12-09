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
                entity.ToTable("RssSources");

                entity.Property(e => e.Id)
                    .HasColumnName("RssSourceId")
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ArticlesXPath)
                    .IsRequired()
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.ArticleTitleXPath)
                    .IsRequired()
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.ArticleLinkXPath)
                    .IsRequired()
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.ArticleLinkPrefix)
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.ArticleDescriptionXPath)
                    .IsRequired()
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.ArticlePubDateXPath)
                    .IsRequired()
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.ArticlePubDateHasAgoPattern)
                    .IsRequired();

                entity.Property(e => e.ArticlePubDateTimeZone)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ArticlePubDateFormat)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ArticlePubDateRegex)
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.Enabled)
                    .IsRequired();
            });
        }
    }
}
