namespace TKMaster.Project.LoginAndSystem.Core.Domain.Models.UsuarioIdentity;

public class ResetPasswordModel
{
    public string Email { get; set; }

    public string Senha { get; set; }

    public string ConfirmeSenha { get; set; }

    public string Code { get; set; }
}