using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EchoShelf_Api.Models;

public partial class DB : DbContext
{
    public DB()
    {
    }

    public DB(DbContextOptions<DB> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<EchoAnalysis> EchoAnalyses { get; set; }

    public virtual DbSet<EchoAnalysisDetail> EchoAnalysisDetails { get; set; }

    public virtual DbSet<Memory> Memories { get; set; }

    public virtual DbSet<MemoryImage> MemoryImages { get; set; }

    public virtual DbSet<MemoryTag> MemoryTags { get; set; }

    public virtual DbSet<Setting> Settings { get; set; }

    public virtual DbSet<Shelf> Shelves { get; set; }

    public virtual DbSet<ShelfItem> ShelfItems { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VwMemorySummary> VwMemorySummaries { get; set; }

    public virtual DbSet<VwShelfSummary> VwShelfSummaries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=localhost;user id=sa;password=P@ssWord;database=EchoShelfDB;trust server certificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A0BA7F5C3D1");

            entity.HasIndex(e => e.CategoryName, "IX_Category_Name");

            entity.HasIndex(e => e.CategoryName, "UQ__Categori__8517B2E08181C777").IsUnique();

            entity.Property(e => e.CategoryName).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<EchoAnalysis>(entity =>
        {
            entity.HasKey(e => e.AnalysisId).HasName("PK__EchoAnal__5B789DC81D3AF2FD");

            entity.HasIndex(e => e.UserId, "IX_EchoAnalyses_User");

            entity.Property(e => e.AnalysisDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Summary).HasMaxLength(500);

            entity.HasOne(d => d.User).WithMany(p => p.EchoAnalyses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EchoAnalyses_User");
        });

        modelBuilder.Entity<EchoAnalysisDetail>(entity =>
        {
            entity.HasKey(e => e.DetailId).HasName("PK__EchoAnal__135C316DF9AB4109");

            entity.HasIndex(e => e.AnalysisId, "IX_EchoAnalysisDetails_Analysis");

            entity.Property(e => e.Comment).HasMaxLength(300);
            entity.Property(e => e.Score).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.Analysis).WithMany(p => p.EchoAnalysisDetails)
                .HasForeignKey(d => d.AnalysisId)
                .HasConstraintName("FK_EchoAnalysisDetails_Analysis");

            entity.HasOne(d => d.Category).WithMany(p => p.EchoAnalysisDetails)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_EchoAnalysisDetails_Category");

            entity.HasOne(d => d.Tag).WithMany(p => p.EchoAnalysisDetails)
                .HasForeignKey(d => d.TagId)
                .HasConstraintName("FK_EchoAnalysisDetails_Tag");
        });

        modelBuilder.Entity<Memory>(entity =>
        {
            entity.HasKey(e => e.MemoryId).HasName("PK__Memories__9A4986D4ED5B6AAF");

            entity.HasIndex(e => e.CategoryId, "IX_Memories_Category");

            entity.HasIndex(e => e.MemoryDate, "IX_Memories_Date");

            entity.HasIndex(e => e.UserId, "IX_Memories_User");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Episode).HasMaxLength(1000);
            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasOne(d => d.Category).WithMany(p => p.Memories)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Memories_Category");

            entity.HasOne(d => d.User).WithMany(p => p.Memories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Memories_User");
        });

        modelBuilder.Entity<MemoryImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__MemoryIm__7516F70C9CF6D103");

            entity.HasIndex(e => e.MemoryId, "IX_MemoryImages_Memory");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.FileName).HasMaxLength(255);
            entity.Property(e => e.FilePath).HasMaxLength(500);

            entity.HasOne(d => d.Memory).WithMany(p => p.MemoryImages)
                .HasForeignKey(d => d.MemoryId)
                .HasConstraintName("FK_MemoryImages_Memory");
        });

        modelBuilder.Entity<MemoryTag>(entity =>
        {
            entity.HasKey(e => e.MemoryTagId).HasName("PK__MemoryTa__7FC68DB16D69B950");

            entity.HasIndex(e => e.MemoryId, "IX_MemoryTags_Memory");

            entity.HasIndex(e => e.TagId, "IX_MemoryTags_Tag");

            entity.HasIndex(e => new { e.MemoryId, e.TagId }, "UQ_Memory_Tag").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Memory).WithMany(p => p.MemoryTags)
                .HasForeignKey(d => d.MemoryId)
                .HasConstraintName("FK_MemoryTags_Memory");

            entity.HasOne(d => d.Tag).WithMany(p => p.MemoryTags)
                .HasForeignKey(d => d.TagId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MemoryTags_Tag");
        });

        modelBuilder.Entity<Setting>(entity =>
        {
            entity.HasKey(e => e.SettingId).HasName("PK__Settings__54372B1DF45CB0CD");

            entity.HasIndex(e => e.SettingKey, "UQ__Settings__01E719ADE10EB8B6").IsUnique();

            entity.Property(e => e.Description).HasMaxLength(300);
            entity.Property(e => e.SettingKey).HasMaxLength(100);
            entity.Property(e => e.SettingValue).HasMaxLength(300);
        });

        modelBuilder.Entity<Shelf>(entity =>
        {
            entity.HasKey(e => e.ShelfId).HasName("PK__Shelves__DBD04F07D48A6949");

            entity.HasIndex(e => e.UserId, "IX_Shelves_User");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description).HasMaxLength(300);
            entity.Property(e => e.DisplayOrder).HasDefaultValue(1);
            entity.Property(e => e.ShelfName).HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.Shelves)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Shelves_User");
        });

        modelBuilder.Entity<ShelfItem>(entity =>
        {
            entity.HasKey(e => e.ShelfItemId).HasName("PK__ShelfIte__2405FF8F55D9D00C");

            entity.HasIndex(e => e.MemoryId, "IX_ShelfItems_Memory");

            entity.HasIndex(e => e.ShelfId, "IX_ShelfItems_Shelf");

            entity.HasIndex(e => new { e.ShelfId, e.MemoryId }, "UQ_Shelf_Memory").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DisplayOrder).HasDefaultValue(1);

            entity.HasOne(d => d.Memory).WithMany(p => p.ShelfItems)
                .HasForeignKey(d => d.MemoryId)
                .HasConstraintName("FK_ShelfItems_Memory");

            entity.HasOne(d => d.Shelf).WithMany(p => p.ShelfItems)
                .HasForeignKey(d => d.ShelfId)
                .HasConstraintName("FK_ShelfItems_Shelf");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.TagId).HasName("PK__Tags__657CF9ACE4B6FD64");

            entity.HasIndex(e => e.TagName, "IX_Tag_Name");

            entity.HasIndex(e => e.TagName, "UQ__Tags__BDE0FD1D4C80104D").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TagName).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CE49C8510");

            entity.HasIndex(e => e.Email, "IX_Users_Email");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534035F2B50").IsUnique();

            entity.Property(e => e.AvatarImagePath).HasMaxLength(300);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        modelBuilder.Entity<VwMemorySummary>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_MemorySummary");

            entity.Property(e => e.CategoryName).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(100);
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        modelBuilder.Entity<VwShelfSummary>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_ShelfSummary");

            entity.Property(e => e.ShelfName).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
