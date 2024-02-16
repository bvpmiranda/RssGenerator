using Microsoft.EntityFrameworkCore;
using System;

namespace RssGenerator.Entities
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
                entity.ToTable("articles");

                entity.Property(e => e.Id)
                    .HasColumnName("articleid")
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.RssSourceId)
                    .HasColumnName(nameof(RssSourceId).ToLower())
                    .IsRequired();

                entity.Property(e => e.Title)
                    .HasColumnName(nameof(Title).ToLower())
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false);

                entity.Property(e => e.Link)
                    .HasColumnName(nameof(Link).ToLower())
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasColumnName(nameof(Description).ToLower())
                    .IsRequired()
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.PubDate)
                    .HasColumnName(nameof(PubDate).ToLower())
                    .IsRequired();

                entity.HasOne(d => d.RssSource)
                    .WithMany(p => p.Articles)
                    .HasForeignKey(d => d.RssSourceId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
