using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ProjetoTcc.Pages
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Redireciona a rota raiz (/) diretamente para a página de Depoimentos
            return RedirectToPage("/Depoimentos");
        }
    }
}
