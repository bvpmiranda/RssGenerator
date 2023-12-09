using Microsoft.EntityFrameworkCore;

namespace RssGenerator.Entities
{
    public class RssSourceArticlePubDateReplace
    {
        public int Id { get; set; }
        public int RssSourceId { get; set; }

        public string Value { get; set; }
        public string Replacement { get; set; }
        
        public RssSource RssSource { get; set; }

        internal static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RssSourceArticlePubDateReplace>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("RssSourceArticlePubDateReplaceId")
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Replacement)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.RssSource)
                    .WithMany(p => p.ArticlePubDateReplace)
                    .HasForeignKey(d => d.RssSourceId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
