using System.ComponentModel.DataAnnotations;

namespace TKMaster.Project.LoginAndSystem.Core.Service.DTOs.UsuarioIdentity;

public class LoginUsuarioRequestDTO
{
    [Required(ErrorMessage = "O campo {0} é obrigatório!")]
    [EmailAddress(ErrorMessage = "E-mail em formato inválido.")]
    [Display(Name = "E-mail")]
    public string Email { get; set; }

    // Password
    [Required(ErrorMessage = "O campo {0} é obrigatório!")]
    [DataType(DataType.Password)]
    [Display(Name = "Senha")]
    public string Senha { get; set; }

    //RememberMe
    [Display(Name = "Lembrar de mim?")]
    public bool MeLembre { get; set; }
}
