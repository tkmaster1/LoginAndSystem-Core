using System.Collections.Generic;

namespace TKMaster.Project.LoginAndSystem.Core.Domain.Models.UsuarioIdentity;

public class UsuarioTokenModel
{
    public string Id { get; set; }

    public string Email { get; set; }

    public string UserName { get; set; }

    public IEnumerable<ClaimModel> Claims { get; set; }
}
