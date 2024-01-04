using Escolar32.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Escolar32.ViewModels
{
    public class CadastroViewModel
    { 
        public Aluno Aluno { get; set; }

        public Escola Escola { get; set; }

        public Bairro Bairro { get; set; }

        public List<SelectListItem> ComboEscolas { get; set; }

        public List<SelectListItem> ComboSeries { get; set; }
        

    }
}