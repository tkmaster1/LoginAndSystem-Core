using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TKMaster.Project.Common.Domain.Entities;

namespace TKMaster.Project.LoginAndSystem.Core.Data.Context;

public class IdentityContext : IdentityDbContext<UsuarioIdentity>
{
    #region Constructor

    public IdentityContext(DbContextOptions<IdentityContext> options)
        : base(options)
    {
        ChangeTracker.LazyLoadingEnabled = false;
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    #endregion

    public DbSet<UsuarioIdentity> Usuarios { get; set; }

    public DbSet<IdentityUserClaim<string>> PermissoesUsuarios { get; set; }

    public DbSet<MenuSistemaEntity> MenuSistemas { get; set; }

    #region ModelBuilder

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UsuarioIdentity>().ToTable("AspNetUsers").HasKey(t => t.Id);

        modelBuilder.Entity<MenuSistemaEntity>().ToTable("MenuSistema").HasKey(t => t.Codigo);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentityContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging(false);
    }

    #endregion
}