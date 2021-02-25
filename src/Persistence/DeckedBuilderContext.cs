using System;
using System.IO;
using System.Reflection;
using Application.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Persistence.Models;

#nullable disable

namespace Persistence
{
    public partial class DeckedBuilderContext : DbContext
    {
        private readonly DeckedBuilderVersion _deckedBuilderVersion;

        public DeckedBuilderContext(DeckedBuilderVersion deckedBuilderVersion) {
            _deckedBuilderVersion = deckedBuilderVersion ?? throw new ArgumentNullException(nameof(deckedBuilderVersion));
        }

        public virtual DbSet<Card> Cards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var filePath = $"Data\\{_deckedBuilderVersion.LatestVersion}\\cards.sqlite";
                optionsBuilder.UseSqlite($"Data Source={Path.Combine(basePath,filePath)}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Card>(entity =>
            {
                entity.HasKey(e => e.Mvid);

                entity.ToTable("card");

                entity.HasIndex(e => new { e.Cardname, e.Ordercmc }, "card_name_ordercmc");

                entity.HasIndex(e => e.Cardname, "ix_card_cardname");

                entity.HasIndex(e => new { e.Cardname, e.Setorder }, "ix_card_cardname_mvid");

                entity.HasIndex(e => new { e.Cardname, e.Cardset }, "ix_card_cardname_set");

                entity.HasIndex(e => e.Convertedmanacost, "ix_card_cmc");

                entity.HasIndex(e => e.Isartifact, "ix_card_isartifact");

                entity.HasIndex(e => e.Isbasicland, "ix_card_isbasicland");

                entity.HasIndex(e => e.Isblack, "ix_card_isblack");

                entity.HasIndex(e => e.Isblue, "ix_card_isblue");

                entity.HasIndex(e => e.Iscolorless, "ix_card_iscolorless");

                entity.HasIndex(e => e.Iscreature, "ix_card_iscreature");

                entity.HasIndex(e => e.Isedh, "ix_card_isedh");

                entity.HasIndex(e => e.Isenchantment, "ix_card_isenchantment");

                entity.HasIndex(e => e.Isextended, "ix_card_isextended");

                entity.HasIndex(e => e.Isgreen, "ix_card_isgreen");

                entity.HasIndex(e => e.Ishistoric, "ix_card_ishistoric");

                entity.HasIndex(e => e.Isinstant, "ix_card_isinstant");

                entity.HasIndex(e => e.Island, "ix_card_island");

                entity.HasIndex(e => e.Islegacy, "ix_card_islegacy");

                entity.HasIndex(e => e.Islegendary, "ix_card_islegendary");

                entity.HasIndex(e => e.Ismodern, "ix_card_ismodern");

                entity.HasIndex(e => e.Ispauper, "ix_card_ispauper");

                entity.HasIndex(e => e.Ispioneer, "ix_card_ispioneer");

                entity.HasIndex(e => e.Isplaneswalker, "ix_card_isplaneswalker");

                entity.HasIndex(e => e.Isred, "ix_card_isred");

                entity.HasIndex(e => e.Issorcery, "ix_card_issorcery");

                entity.HasIndex(e => e.Isstandard, "ix_card_isstandard");

                entity.HasIndex(e => e.Iswhite, "ix_card_iswhite");

                entity.HasIndex(e => e.Powerint, "ix_card_powerint");

                entity.HasIndex(e => e.Prodany, "ix_card_prodany");

                entity.HasIndex(e => e.Prodblack, "ix_card_prodblack");

                entity.HasIndex(e => e.Prodblue, "ix_card_prodblue");

                entity.HasIndex(e => e.Prodcolorless, "ix_card_prodcolorless");

                entity.HasIndex(e => e.Prodgreen, "ix_card_prodgreen");

                entity.HasIndex(e => e.Prodred, "ix_card_prodred");

                entity.HasIndex(e => e.Prodwhite, "ix_card_prodwhite");

                entity.HasIndex(e => e.Rarity, "ix_card_rarity");

                entity.HasIndex(e => e.Toughnessint, "ix_card_toughnessint");

                entity.HasIndex(e => new { e.Convertedmanacost, e.Cardname, e.Setorder }, "ix_sort1");

                entity.HasIndex(e => new { e.Cardname, e.Setorder }, "ix_sort2");

                entity.HasIndex(e => new { e.Colororder, e.Cardname, e.Setorder }, "ix_sort3");

                entity.Property(e => e.Mvid)
                    .HasColumnType("int")
                    .ValueGeneratedNever()
                    .HasColumnName("mvid");

                entity.Property(e => e.Artist)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("artist");

                entity.Property(e => e.Cardname)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("cardname");

                entity.Property(e => e.Cardnumber)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("cardnumber");

                entity.Property(e => e.Cardset)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("cardset");

                entity.Property(e => e.Cardtext)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("cardtext");

                entity.Property(e => e.Cardtype)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("cardtype");

                entity.Property(e => e.Colororder)
                    .HasColumnType("int")
                    .HasColumnName("colororder");

                entity.Property(e => e.Convertedmanacost)
                    .HasColumnType("int")
                    .HasColumnName("convertedmanacost");

                entity.Property(e => e.Gathererid)
                    .HasColumnType("int")
                    .HasColumnName("gathererid");

                entity.Property(e => e.HdartMd5)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("hdart_md5");

                entity.Property(e => e.HdartPath)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("hdart_path");

                entity.Property(e => e.Isartifact)
                    .HasColumnType("int")
                    .HasColumnName("isartifact");

                entity.Property(e => e.Isbasicland)
                    .HasColumnType("int")
                    .HasColumnName("isbasicland");

                entity.Property(e => e.Isblack)
                    .HasColumnType("int")
                    .HasColumnName("isblack");

                entity.Property(e => e.Isblue)
                    .HasColumnType("int")
                    .HasColumnName("isblue");

                entity.Property(e => e.Iscolorless)
                    .HasColumnType("int")
                    .HasColumnName("iscolorless");

                entity.Property(e => e.Iscreature)
                    .HasColumnType("int")
                    .HasColumnName("iscreature");

                entity.Property(e => e.Isedh)
                    .HasColumnType("int")
                    .HasColumnName("isedh");

                entity.Property(e => e.Isenchantment)
                    .HasColumnType("int")
                    .HasColumnName("isenchantment");

                entity.Property(e => e.Isextended)
                    .HasColumnType("int")
                    .HasColumnName("isextended");

                entity.Property(e => e.Isgreen)
                    .HasColumnType("int")
                    .HasColumnName("isgreen");

                entity.Property(e => e.Ishistoric)
                    .HasColumnType("int")
                    .HasColumnName("ishistoric");

                entity.Property(e => e.Isinstant)
                    .HasColumnType("int")
                    .HasColumnName("isinstant");

                entity.Property(e => e.Island)
                    .HasColumnType("int")
                    .HasColumnName("island");

                entity.Property(e => e.Islegacy)
                    .HasColumnType("int")
                    .HasColumnName("islegacy");

                entity.Property(e => e.Islegendary)
                    .HasColumnType("int")
                    .HasColumnName("islegendary");

                entity.Property(e => e.Ismodern)
                    .HasColumnType("int")
                    .HasColumnName("ismodern");

                entity.Property(e => e.Ispauper)
                    .HasColumnType("int")
                    .HasColumnName("ispauper");

                entity.Property(e => e.Ispioneer)
                    .HasColumnType("int")
                    .HasColumnName("ispioneer");

                entity.Property(e => e.Isplaneswalker)
                    .HasColumnType("int")
                    .HasColumnName("isplaneswalker");

                entity.Property(e => e.Isred)
                    .HasColumnType("int")
                    .HasColumnName("isred");

                entity.Property(e => e.Issorcery)
                    .HasColumnType("int")
                    .HasColumnName("issorcery");

                entity.Property(e => e.Isstandard)
                    .HasColumnType("int")
                    .HasColumnName("isstandard");

                entity.Property(e => e.Iswhite)
                    .HasColumnType("int")
                    .HasColumnName("iswhite");

                entity.Property(e => e.Manacost)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("manacost");

                entity.Property(e => e.Manasymbols)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("manasymbols");

                entity.Property(e => e.Ordercmc)
                    .HasColumnType("int")
                    .HasColumnName("ordercmc");

                entity.Property(e => e.Power)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("power");

                entity.Property(e => e.Powerint)
                    .HasColumnType("int")
                    .HasColumnName("powerint");

                entity.Property(e => e.Prodany)
                    .HasColumnType("int")
                    .HasColumnName("prodany");

                entity.Property(e => e.Prodblack)
                    .HasColumnType("int")
                    .HasColumnName("prodblack");

                entity.Property(e => e.Prodblue)
                    .HasColumnType("int")
                    .HasColumnName("prodblue");

                entity.Property(e => e.Prodcolorless)
                    .HasColumnType("int")
                    .HasColumnName("prodcolorless");

                entity.Property(e => e.Prodgreen)
                    .HasColumnType("int")
                    .HasColumnName("prodgreen");

                entity.Property(e => e.Prodred)
                    .HasColumnType("int")
                    .HasColumnName("prodred");

                entity.Property(e => e.Prodwhite)
                    .HasColumnType("int")
                    .HasColumnName("prodwhite");

                entity.Property(e => e.Rarity)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("rarity");

                entity.Property(e => e.Scryfallid)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("scryfallid");

                entity.Property(e => e.SdartMd5)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("sdart_md5");

                entity.Property(e => e.SdartPath)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("sdart_path");

                entity.Property(e => e.Setorder)
                    .HasColumnType("int")
                    .HasColumnName("setorder");

                entity.Property(e => e.Specialtype)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("specialtype");

                entity.Property(e => e.ThumbMd5)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("thumb_md5");

                entity.Property(e => e.ThumbPath)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("thumb_path");

                entity.Property(e => e.Toughness)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("toughness");

                entity.Property(e => e.Toughnessint)
                    .HasColumnType("int")
                    .HasColumnName("toughnessint");

                entity.Property(e => e.Transformid)
                    .HasColumnType("int")
                    .HasColumnName("transformid");

                entity.Property(e => e.Typeline)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("typeline");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
