﻿using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using PROG2500_Final.Models;

namespace PROG2500_Final.Data;
//scaffold-dbcontext "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=IMDB_Project;Integrated Security=true;" Microsoft.EntityFrameworkCore.SqlServer -ContextDir Data -OutputDir Models/Generated -DataAnnotations -ContextNamespace PROG2500_Final.Data -Namespace PROG2500_Final.Models

public partial class ImdbProjectContext : DbContext
{
    public ImdbProjectContext()
    {
    }

    public ImdbProjectContext(DbContextOptions<ImdbProjectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Episode> Episodes { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Name> Names { get; set; }

    public virtual DbSet<Principal> Principals { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Title> Titles { get; set; }

    public virtual DbSet<TitleAlias> TitleAliases { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["IMDBDatabase"].ConnectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Episode>(entity =>
        {
            entity.HasOne(d => d.ParentTitle).WithMany(p => p.EpisodeParentTitles).HasConstraintName("FK_Episodes_Titles1");

            entity.HasOne(d => d.Title).WithOne(p => p.EpisodeTitle)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Episodes_Titles");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.GenreId).HasName("PK_Title_Genres");
        });

        modelBuilder.Entity<Principal>(entity =>
        {
            entity.HasOne(d => d.Name).WithMany(p => p.Principals).HasConstraintName("FK_Principals_Names");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasOne(d => d.Title).WithOne(p => p.Rating)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ratings_Titles");
        });

        modelBuilder.Entity<Title>(entity =>
        {
            entity.HasMany(d => d.Genres).WithMany(p => p.Titles)
                .UsingEntity<Dictionary<string, object>>(
                    "TitleGenre",
                    r => r.HasOne<Genre>().WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Title_Genres_Titles"),
                    l => l.HasOne<Title>().WithMany()
                        .HasForeignKey("TitleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Title_Genres_Titles1"),
                    j =>
                    {
                        j.HasKey("TitleId", "GenreId").HasName("PK_Title_Genres_1");
                        j.ToTable("Title_Genres");
                        j.IndexerProperty<string>("TitleId")
                            .HasMaxLength(10)
                            .IsUnicode(false)
                            .HasColumnName("titleID");
                        j.IndexerProperty<int>("GenreId").HasColumnName("genreID");
                    });

            entity.HasMany(d => d.Names).WithMany(p => p.Titles)
                .UsingEntity<Dictionary<string, object>>(
                    "Director",
                    r => r.HasOne<Name>().WithMany()
                        .HasForeignKey("NameId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Directors_Names"),
                    l => l.HasOne<Title>().WithMany()
                        .HasForeignKey("TitleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Directors_Titles1"),
                    j =>
                    {
                        j.HasKey("TitleId", "NameId");
                        j.ToTable("Directors");
                        j.IndexerProperty<string>("TitleId")
                            .HasMaxLength(10)
                            .IsUnicode(false)
                            .HasColumnName("titleID");
                        j.IndexerProperty<string>("NameId")
                            .HasMaxLength(10)
                            .IsUnicode(false)
                            .HasColumnName("nameID");
                    });

            entity.HasMany(d => d.Names1).WithMany(p => p.Titles1)
                .UsingEntity<Dictionary<string, object>>(
                    "Writer",
                    r => r.HasOne<Name>().WithMany()
                        .HasForeignKey("NameId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Writers_Names"),
                    l => l.HasOne<Title>().WithMany()
                        .HasForeignKey("TitleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Writers_Titles"),
                    j =>
                    {
                        j.HasKey("TitleId", "NameId");
                        j.ToTable("Writers");
                        j.IndexerProperty<string>("TitleId")
                            .HasMaxLength(10)
                            .IsUnicode(false)
                            .HasColumnName("titleID");
                        j.IndexerProperty<string>("NameId")
                            .HasMaxLength(10)
                            .IsUnicode(false)
                            .HasColumnName("nameID");
                    });

            entity.HasMany(d => d.NamesNavigation).WithMany(p => p.TitlesNavigation)
                .UsingEntity<Dictionary<string, object>>(
                    "KnownFor",
                    r => r.HasOne<Name>().WithMany()
                        .HasForeignKey("NameId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Known_For_Names"),
                    l => l.HasOne<Title>().WithMany()
                        .HasForeignKey("TitleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Known_For_Titles"),
                    j =>
                    {
                        j.HasKey("TitleId", "NameId").HasName("PK_KnownFor");
                        j.ToTable("Known_For");
                        j.IndexerProperty<string>("TitleId")
                            .HasMaxLength(10)
                            .IsUnicode(false)
                            .HasColumnName("titleID");
                        j.IndexerProperty<string>("NameId")
                            .HasMaxLength(10)
                            .IsUnicode(false)
                            .HasColumnName("nameID");
                    });
        });

        modelBuilder.Entity<TitleAlias>(entity =>
        {
            entity.HasKey(e => new { e.TitleId, e.Ordering }).HasName("PK_Title_AKAs");

            entity.HasOne(d => d.TitleNavigation).WithMany(p => p.TitleAliases)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Title_Aliases_Titles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
