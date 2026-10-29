namespace ProjetoTcc.Models
{
    public class Depoimentos
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string FaseDiagnostico { get; set; } = string.Empty;
        public string Conteudo { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }
}