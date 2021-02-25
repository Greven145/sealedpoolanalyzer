﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Persistence;

namespace Persistence.Migrations
{
    [DbContext(typeof(AnalyzerContext))]
    partial class AnalyzerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.3");

            modelBuilder.Entity("Domain.Entities.MagicCard", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Colors")
                        .HasColumnType("TEXT");

                    b.Property<string>("ManaCost")
                        .HasColumnType("TEXT");

                    b.Property<string>("MultiverseIds")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SetId")
                        .HasColumnType("TEXT");

                    b.Property<string>("TypeLine")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SetId");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("Domain.Entities.MagicCardReview", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("MagicCardId")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Score")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("MagicCardId")
                        .IsUnique();

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("Domain.Entities.Set", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Code")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("DateLoaded")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Uri")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Sets");
                });

            modelBuilder.Entity("Persistence.Models.DeckedBuilderToScryfallRelationship", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("Scryfallid")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("DB2Scryfall");
                });

            modelBuilder.Entity("Domain.Entities.MagicCard", b =>
                {
                    b.HasOne("Domain.Entities.Set", "Set")
                        .WithMany("MagicCards")
                        .HasForeignKey("SetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Set");
                });

            modelBuilder.Entity("Domain.Entities.MagicCardReview", b =>
                {
                    b.HasOne("Domain.Entities.MagicCard", "Card")
                        .WithOne("Review")
                        .HasForeignKey("Domain.Entities.MagicCardReview", "MagicCardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Card");
                });

            modelBuilder.Entity("Domain.Entities.MagicCard", b =>
                {
                    b.Navigation("Review");
                });

            modelBuilder.Entity("Domain.Entities.Set", b =>
                {
                    b.Navigation("MagicCards");
                });
#pragma warning restore 612, 618
        }
    }
}