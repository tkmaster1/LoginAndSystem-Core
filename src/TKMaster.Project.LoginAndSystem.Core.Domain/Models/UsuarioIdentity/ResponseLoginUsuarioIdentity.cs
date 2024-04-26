namespace TKMaster.Project.LoginAndSystem.Core.Domain.Models.UsuarioIdentity;

public class ResponseLoginUsuarioIdentity
{
    public string AccessToken { get; set; }

    public double ExpiresIn { get; set; }

    public UsuarioTokenModel UserToken { get; set; }
}
