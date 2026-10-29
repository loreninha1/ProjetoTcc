using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjetoTcc.Data;
using ProjetoTcc.Models;

namespace ProjetoTcc.Pages
{
    public class DepoimentosModel : PageModel
    {
        private readonly DepoimentosDB _db;

        public DepoimentosModel(DepoimentosDB db)
        {
            _db = db;
        }

        public List<Depoimentos> ListaDepoimentos { get; set; } = new();

        public void OnGet()
        {
            ListaDepoimentos = _db.ObterTodos();
        }

        public IActionResult OnPost(string Titulo, string FaseDiagnostico, string Conteudo)
        {
            if (!string.IsNullOrWhiteSpace(Titulo) && !string.IsNullOrWhiteSpace(Conteudo))
            {
                var novoDepoimento = new Depoimentos
                {
                    Titulo = Titulo,
                    FaseDiagnostico = FaseDiagnostico,
                    Conteudo = Conteudo
                };

                _db.Inserir(novoDepoimento);
            }

            return RedirectToPage();
        }
    }
}