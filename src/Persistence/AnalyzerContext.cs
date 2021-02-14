using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence {
    public class AnalyzerContext : DbContext {
        public DbSet<MagicCard> Cards { get; set; }
        public DbSet<Set> Sets { get; set; }
        public DbSet<MagicCardReview> Reviews { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options) {
            options.UseSqlite("Data Source=cards.db");
        }
    }
}
