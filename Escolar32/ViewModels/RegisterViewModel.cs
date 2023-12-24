using System.ComponentModel.DataAnnotations;

namespace Escolar32.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Informe um email válido...")]
        [Display(Name = "Email")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "A senha deverá conter números, letras maiúsculas, minúsculas e caracteres especiais")]
        [DataType(DataType.Password)]
        [Display(Name = "Crie uma senha com números, letras maiúsculas, minúsculas e caracteres especiais (@#$%&*)")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirme a senha")]
        [Compare("Password", ErrorMessage = "A senha e a senha de confirmação NÃO conferem")]
        public string ConfirmPassword { get; set; }
        public string ReturnUrl { get; set; }
    }
}
