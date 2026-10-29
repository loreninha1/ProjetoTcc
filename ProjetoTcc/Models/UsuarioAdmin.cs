namespace ProjetoTcc.Models
{
    public class UsuarioAdmin
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty; // Em produção, armazena-se o Hash
    }
}
