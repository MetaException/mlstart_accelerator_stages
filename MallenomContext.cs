using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using stage3.Model;

namespace stage3;

public partial class MallenomContext : DbContext
{
    public MallenomContext()
    {
    }

    public MallenomContext(DbContextOptions<MallenomContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=mallenom;Username=postgres;Password=4409");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("users");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.LoginHash)
                .HasMaxLength(255)
                .HasColumnName("login_hash");
            entity.Property(e => e.PassHash)
                .HasMaxLength(255)
                .HasColumnName("pass_hash");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
