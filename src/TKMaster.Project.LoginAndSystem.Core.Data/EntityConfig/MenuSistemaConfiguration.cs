using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TKMaster.Project.Common.Domain.Entities;

namespace TKMaster.Project.LoginAndSystem.Core.Data.EntityConfig;

public class MenuSistemaConfiguration : IEntityTypeConfiguration<MenuSistemaEntity>
{
    public void Configure(EntityTypeBuilder<MenuSistemaEntity> builder)
    {
        builder.ToTable("MenuSistema", "dbo");

        builder
            .Property(p => p.Codigo)
            .HasColumnName("Id");

        builder
            .HasKey(p => p.Codigo)
            .HasAnnotation("SqlServer:Identity", "1, 1")
            .HasName("PK_MenuSistema");

        builder
            .Property(p => p.Nome)
            .HasColumnType("varchar(256)")
            .IsRequired()
            .HasColumnName("Nome");

        builder
            .Property(p => p.Descricao)
            .HasColumnType("varchar(max)")
            .IsRequired()
            .HasColumnName("Descricao");

        builder
            .Property(p => p.Status)
            .IsRequired()
            .HasColumnName("Status");
    }
}