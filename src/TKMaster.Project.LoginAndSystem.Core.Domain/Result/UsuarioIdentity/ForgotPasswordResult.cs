namespace TKMaster.Project.LoginAndSystem.Core.Domain.Result.UsuarioIdentity;

public class ForgotPasswordResult
{
    public string CodigoUsuario { get; set; }

    public string Email { get; set; }

    public string Token { get; set; }
}