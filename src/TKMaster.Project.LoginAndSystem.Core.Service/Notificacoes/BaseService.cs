using FluentValidation;
using FluentValidation.Results;
using TKMaster.Project.Common.Domain.Entities;
using TKMaster.Project.LoginAndSystem.Core.Domain.Interfaces.Notifications;
using TKMaster.Project.LoginAndSystem.Core.Domain.Notifications;

namespace TKMaster.Project.LoginAndSystem.Core.Service.Notificacoes;

public abstract class BaseService
{
    #region Properties

    private readonly INotificador _notificador;

    #endregion

    #region Constructor

    protected BaseService(INotificador notificador)
    {
        _notificador = notificador;
    }

    #endregion

    #region Methods

    protected void Notificar(ValidationResult validationResult)
    {
        foreach (var error in validationResult.Errors)
        {
            Notificar(error.ErrorMessage);
        }
    }

    protected void Notificar(string mensagem)
    {
        _notificador.Handle(new Notificacao(mensagem));
    }

    protected bool ExecutarValidacao<TV, TE>(TV validacao, TE entidade) where TV : AbstractValidator<TE> where TE : Entity
    {
        var validator = validacao.Validate(entidade);

        if (validator.IsValid) return true;

        Notificar(validator);

        return false;
    }

    #endregion
}

