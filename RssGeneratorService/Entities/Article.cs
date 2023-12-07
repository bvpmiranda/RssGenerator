using Microsoft.EntityFrameworkCore;
using System;

namespace RssGeneratorService.Entities
{
    public class Article
    {
        public Guid Id { get; set; }
        public int RssSourceId { get; set; }

        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public DateTime PubDate { get; set; }

        public RssSource RssSource { get; set; }

        internal static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ArticleId")
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false);

                entity.Property(e => e.Link)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.PubDate)
                    .IsRequired();

                entity.HasOne(d => d.RssSource)
                    .WithMany(p => p.Articles)
                    .HasForeignKey(d => d.RssSourceId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
