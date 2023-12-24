using System.ComponentModel.DataAnnotations;

namespace Escolar32.ViewModels
{

    public class LoginViewModel
    {

        [Required(ErrorMessage ="Informe o e-mail")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Informe a Senha")]
        [DataType(DataType.Password)]
        [Display(Name ="Senha")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }

    }
}
