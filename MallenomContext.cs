using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using stage3.Model;

namespace stage3;

public partial class MallenomContext : DbContext
{
    private readonly IConfiguration _config;

    public MallenomContext(IConfiguration config)
    {
        _config = config;
    }

    public MallenomContext(DbContextOptions<MallenomContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(_config.GetRequiredSection("ConnectionStrings").GetValue<string>("DbConnectionUrl"));

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
