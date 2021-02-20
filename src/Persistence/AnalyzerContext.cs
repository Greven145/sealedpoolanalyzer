using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Models;

namespace Persistence {
    public class AnalyzerContext : DbContext {
        public DbSet<MagicCard> Cards { get; set; }
        public DbSet<Set> Sets { get; set; }
        public DbSet<MagicCardReview> Reviews { get; set; }
        public DbSet<DeckedBuilderToScryfallRelationship> DB2Scryfall { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options) {
            if (!options.IsConfigured) {
                options.UseSqlite("Data Source=cards.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<MagicCard>()
                .Property(c => c.Colors)
                .HasConversion(
                    c => JsonSerializer.Serialize(c,null),
                    c => JsonSerializer.Deserialize<IEnumerable<string>>(c,null));
            modelBuilder.Entity<MagicCard>()
                .Property(c => c.MultiverseIds)
                .HasConversion(
                    m => JsonSerializer.Serialize(m,null),
                    m => JsonSerializer.Deserialize<IEnumerable<int>>(m,null));
            modelBuilder.Entity<DeckedBuilderToScryfallRelationship>()
                .HasKey(d => d.Id);
            modelBuilder.Entity<DeckedBuilderToScryfallRelationship>()
                .Property(d => d.Id)
                .ValueGeneratedNever();
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
