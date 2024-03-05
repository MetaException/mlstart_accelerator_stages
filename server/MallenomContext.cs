using Microsoft.EntityFrameworkCore;
using server.Models;

namespace server;

public partial class MallenomContext : DbContext
{
    public MallenomContext(DbContextOptions<MallenomContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    public virtual DbSet<LoginModel> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LoginModel>(entity =>
        {
            entity
                ///.HasNoKey()
                .ToTable("users");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .HasColumnName("login_hash");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("pass_hash");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
