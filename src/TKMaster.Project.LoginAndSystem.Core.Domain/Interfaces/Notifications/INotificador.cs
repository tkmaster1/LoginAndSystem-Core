using System.Collections.Generic;
using TKMaster.Project.LoginAndSystem.Core.Domain.Notifications;

namespace TKMaster.Project.LoginAndSystem.Core.Domain.Interfaces.Notifications;

public interface INotificador
{
    bool TemNotificacao();

    List<Notificacao> ObterNotificacoes();

    void Handle(Notificacao notificacao);
}