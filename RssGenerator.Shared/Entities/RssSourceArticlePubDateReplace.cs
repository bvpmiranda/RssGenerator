using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

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
                entity.ToTable("rsssourcearticlepubdatereplaces");

                entity.Property(e => e.Id)
                    .HasColumnName("rsssourcearticlepubdatereplaceid")
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.RssSourceId)
                    .HasColumnName(nameof(RssSourceId).ToLower())
                    .IsRequired();

                entity.Property(e => e.Value)
                    .HasColumnName(nameof(Value).ToLower())
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Replacement)
                    .HasColumnName(nameof(Replacement).ToLower())
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
