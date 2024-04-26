using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TKMaster.Project.Common.Domain.Response;
using TKMaster.Project.LoginAndSystem.Core.Domain.Interfaces.Notifications;
using TKMaster.Project.LoginAndSystem.Core.Domain.Interfaces.Services;

namespace TKMaster.Project.LoginAndSystem.Core.WebApi.Controllers;

[ApiController]
public abstract class MainAPIController : ControllerBase
{
    #region Properties

    protected ICollection<string> Erros = new List<string>();

    private readonly INotificador _notificador;

    public readonly IUserAppService _appUser;

    protected Guid UsuarioId { get; set; }

    protected bool UsuarioAutenticado { get; set; }

    #endregion

    #region Constructor

    protected MainAPIController(INotificador notificador,
                                IUserAppService appUser)
    {
        _notificador = notificador;
        _appUser = appUser;

        if (appUser.IsAuthenticated())
        {
            UsuarioId = appUser.GetUserId();
            UsuarioAutenticado = true;
        }
    }

    #endregion

    #region Methods Protecteds

    protected ActionResult CustomResponse(object result = null, bool? error = false, string message = null)
    {
        if (error != null && error == true)
        {
            return Conflict(new ResponseFailure
            {
                Success = false,
                Errors = Erros.Append(message)
            });
        }

        if (OperacaoValida())
        {
            return Ok(new ResponseSuccess<object>
            {
                Success = true,
                Data = result
            });
        }

        return Conflict(new ResponseFailure
        {
            Success = false,
            Errors = (IEnumerable<string>)_notificador.ObterNotificacoes()
        });
    }

    protected bool ResponsePossuiErros(ResponseResult resposta)
    {
        if (resposta == null || !resposta.Errors.Mensagens.Any()) return false;

        foreach (var mensagem in resposta.Errors.Mensagens)
        {
            AdicionarErroProcessamento(mensagem);
        }

        return true;
    }

    protected bool OperacaoValida()
    {
        return !_notificador.TemNotificacao();
        //return !Erros.Any();
    }

    protected void AdicionarErroProcessamento(string erro)
    {
        Erros.Add(erro);
    }

    protected void LimparErrosProcessamento()
    {
        Erros.Clear();
    }

    #endregion    
}
