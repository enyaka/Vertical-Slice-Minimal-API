using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using VerticalMinimalApi.Models;

using VerticalMinimalApi.Options;
namespace VerticalMinimalApi.Context;

public partial class MinimalDbContext : DbContext
{
    private readonly IOptions<ConnectionStrings> _connectionStrings;
    public MinimalDbContext(IOptions<ConnectionStrings> connectionStrings)
    {
        _connectionStrings = connectionStrings;
    }

    public MinimalDbContext(DbContextOptions<MinimalDbContext> options, IOptions<ConnectionStrings> connectionStrings)
        : base(options)
    {
        _connectionStrings = connectionStrings;
    }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(_connectionStrings.Value.DefaultConnection);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_pk");

            entity.ToTable("user");

            entity.HasIndex(e => e.Email, "user_un").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.Surname)
                .HasMaxLength(50)
                .HasColumnName("surname");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
