namespace TKMaster.Project.LoginAndSystem.Core.Domain.Result.UsuarioIdentity;

public class LoginUsuarioIdentityResult
{
    public bool Succeeded { get; set; }

    public bool IsLockedOut { get; set; }

    public bool IsNotAllowed { get; set; }

    public bool RequiresTwoFactor { get; set; }

    public string Mensagem { get; set; }
}
