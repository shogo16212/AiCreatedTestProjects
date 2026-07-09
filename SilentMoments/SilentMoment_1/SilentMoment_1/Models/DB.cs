using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SilentMoment_1.Models;

public partial class DB : DbContext
{
    public DB()
    {
    }

    public DB(DbContextOptions<DB> options)
        : base(options)
    {
    }

    public virtual DbSet<Place> Places { get; set; }

    public virtual DbSet<QuietMoment> QuietMoments { get; set; }

    public virtual DbSet<Route> Routes { get; set; }

    public virtual DbSet<RouteMoment> RouteMoments { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=localhost;user id=sa;password=P@ssWord;database=SilentMomentsDB;trust server certificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Place>(entity =>
        {
            entity.HasKey(e => e.PlaceId).HasName("PK__Places__D5222B6E27689A1C");

            entity.HasIndex(e => e.PlaceName, "IX_Places_PlaceName");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PlaceName).HasMaxLength(50);
        });

        modelBuilder.Entity<QuietMoment>(entity =>
        {
            entity.HasKey(e => e.MomentId).HasName("PK__QuietMom__D89D9A4CEDE8C6CA");

            entity.HasIndex(e => e.PlaceId, "IX_QuietMoments_PlaceId");

            entity.HasIndex(e => e.RecordedAt, "IX_QuietMoments_RecordedAt");

            entity.Property(e => e.Memo).HasMaxLength(500);
            entity.Property(e => e.PhotoUrl).HasMaxLength(200);
            entity.Property(e => e.RecordedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasOne(d => d.Place).WithMany(p => p.QuietMoments)
                .HasForeignKey(d => d.PlaceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__QuietMome__Place__3F466844");

            entity.HasMany(d => d.Tags).WithMany(p => p.Moments)
                .UsingEntity<Dictionary<string, object>>(
                    "QuietMomentTag",
                    r => r.HasOne<Tag>().WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__QuietMome__TagId__5165187F"),
                    l => l.HasOne<QuietMoment>().WithMany()
                        .HasForeignKey("MomentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__QuietMome__Momen__5070F446"),
                    j =>
                    {
                        j.HasKey("MomentId", "TagId").HasName("PK__QuietMom__0ECA55D6F81EBE50");
                        j.ToTable("QuietMomentTags");
                        j.HasIndex(new[] { "TagId" }, "IX_QuietMomentTags_TagId");
                    });
        });

        modelBuilder.Entity<Route>(entity =>
        {
            entity.HasKey(e => e.RouteId).HasName("PK__Routes__80979B4D17552FA2");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RouteName).HasMaxLength(100);
        });

        modelBuilder.Entity<RouteMoment>(entity =>
        {
            entity.HasKey(e => new { e.RouteId, e.MomentId }).HasName("PK__RouteMom__5D1E42E9439B51AD");

            entity.HasIndex(e => e.RouteId, "IX_RouteMoments_RouteId");

            entity.HasOne(d => d.Moment).WithMany(p => p.RouteMoments)
                .HasForeignKey(d => d.MomentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RouteMome__Momen__5629CD9C");

            entity.HasOne(d => d.Route).WithMany(p => p.RouteMoments)
                .HasForeignKey(d => d.RouteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RouteMome__Route__5535A963");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.TagId).HasName("PK__Tags__657CF9AC09B8A0E9");

            entity.HasIndex(e => e.TagName, "IX_Tags_TagName");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TagName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
