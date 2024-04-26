using System.ComponentModel.DataAnnotations;
using TKMaster.Project.Common.Domain.Entities;

namespace TKMaster.Project.LoginAndSystem.Core.Domain.Extensions;

public static class MenuSistemaExtensions
{
    public static void ValidarNome(this MenuSistemaEntity menuSistemaEntity)
    {
        if (string.IsNullOrEmpty(menuSistemaEntity.Nome))
            throw new ValidationException("O nome do Menu de Sistema é obrigatório.");
    }

    public static void ValidarDescricao(this MenuSistemaEntity menuSistemaEntity)
    {
        if (string.IsNullOrEmpty(menuSistemaEntity.Descricao))
            throw new ValidationException("A descrição é obrigatória.");
    }

    public static void ValidarMenuSistemaId(this MenuSistemaEntity menuSistemaEntity)
    {
        if (menuSistemaEntity.Codigo == 0)
            throw new ValidationException("O código do Menu de Sistema é obrigatório.");
    }
}