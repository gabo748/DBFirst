using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace DatabaseFirstGabrielCampos.Models;

public partial class DbfirstgcContext : DbContext
{
    public DbfirstgcContext()
    {
    }

    public DbfirstgcContext(DbContextOptions<DbfirstgcContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Match> Matches { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseMySql(connectionString,Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.34-mysql"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Match>(entity =>
        {
            entity.HasKey(e => e.MatchId).HasName("PRIMARY");

            entity.ToTable("matches");

            entity.HasIndex(e => e.AwayTeamId, "AwayTeamID");

            entity.HasIndex(e => e.HomeTeamId, "HomeTeamID");

            entity.Property(e => e.MatchId).HasColumnName("MatchID");
            entity.Property(e => e.AwayScore).HasDefaultValueSql("'0'");
            entity.Property(e => e.AwayTeamId).HasColumnName("AwayTeamID");
            entity.Property(e => e.HomeScore).HasDefaultValueSql("'0'");
            entity.Property(e => e.HomeTeamId).HasColumnName("HomeTeamID");

            entity.HasOne(d => d.AwayTeam).WithMany(p => p.MatchAwayTeams)
                .HasForeignKey(d => d.AwayTeamId)
                .HasConstraintName("matches_ibfk_2");

            entity.HasOne(d => d.HomeTeam).WithMany(p => p.MatchHomeTeams)
                .HasForeignKey(d => d.HomeTeamId)
                .HasConstraintName("matches_ibfk_1");
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.PlayerId).HasName("PRIMARY");

            entity.ToTable("players");

            entity.HasIndex(e => e.TeamId, "TeamID");

            entity.Property(e => e.PlayerId).HasColumnName("PlayerID");
            entity.Property(e => e.PlayerName).HasMaxLength(100);
            entity.Property(e => e.Position).HasMaxLength(50);
            entity.Property(e => e.TeamId).HasColumnName("TeamID");

            entity.HasOne(d => d.Team).WithMany(p => p.Players)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("players_ibfk_1");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.TeamId).HasName("PRIMARY");

            entity.ToTable("teams");

            entity.Property(e => e.TeamId).HasColumnName("TeamID");
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.TeamName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
