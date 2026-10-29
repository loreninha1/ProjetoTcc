using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjetoTcc.Data;
using ProjetoTcc.Models;

namespace ProjetoTcc.Pages
{
    public class AdminModel : PageModel
    {
        private readonly DepoimentosDB _db;

        public AdminModel(DepoimentosDB db)
        {
            _db = db;
        }

        public bool IsLogado { get; set; } = false;
        public string MensagemErro { get; set; } = string.Empty;
        public string MensagemSucesso { get; set; } = string.Empty;
        public List<Depoimentos> ListaDepoimentos { get; set; } = new();

        public void OnGet()
        {
            CheckLogin();
            if (IsLogado)
            {
                ListaDepoimentos = _db.ObterTodos();
            }
        }

        public IActionResult OnPostLogin(string Email, string Senha)
        {
            if (_db.ValidarAdmin(Email, Senha))
            {
                HttpContext.Session.SetString("UsuarioAdmin", Email);
                return RedirectToPage();
            }

            MensagemErro = "E-mail ou senha inválidos.";
            return Page();
        }

        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Remove("UsuarioAdmin");
            return RedirectToPage();
        }

        public IActionResult OnPostExcluir(int id)
        {
            CheckLogin();
            if (IsLogado)
            {
                _db.ExcluirDepoimento(id);
            }
            return RedirectToPage();
        }

        public IActionResult OnPostAlterarSenha(string NovaSenha, string ConfirmarSenha)
        {
            CheckLogin();
            if (!IsLogado) return RedirectToPage();

            if (string.IsNullOrWhiteSpace(NovaSenha) || NovaSenha != ConfirmarSenha)
            {
                MensagemErro = "As senhas não coincidem ou são inválidas.";
                ListaDepoimentos = _db.ObterTodos();
                return Page();
            }

            var emailAdmin = HttpContext.Session.GetString("UsuarioAdmin");
            if (_db.AlterarSenhaAdmin(emailAdmin!, NovaSenha))
            {
                MensagemSucesso = "Senha alterada com sucesso! Utilize a nova senha no próximo acesso.";
            }

            ListaDepoimentos = _db.ObterTodos();
            return Page();
        }

        private void CheckLogin()
        {
            IsLogado = !string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioAdmin"));
        }
    }
}
