namespace TKMaster.Project.LoginAndSystem.Core.Domain.Models.UsuarioIdentity;

public class LoginUsuarioModel
{
    public string Email { get; set; }

    // Password
    public string Senha { get; set; }

    // RememberMe
    public bool MeLembre { get; set; }
}