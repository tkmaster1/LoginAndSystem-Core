using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace TKMaster.Project.LoginAndSystem.Core.Data.Context;

public class MeuContexto : DbContext
{
    #region Constructor

    public MeuContexto(DbContextOptions options) : base(options)
    {
        ChangeTracker.LazyLoadingEnabled = false;
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    #endregion

    #region DBSets

    // Outras tabelas do Sistema
   
    #endregion

    #region ModelBuilder e SaveChanges

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var property in modelBuilder.Model.GetEntityTypes()
           .SelectMany(e => e.GetProperties()
               .Where(p => p.ClrType == typeof(string))))
            property.SetColumnType("varchar(256)");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MeuContexto).Assembly);

        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging(false);
    }

    #endregion
}
