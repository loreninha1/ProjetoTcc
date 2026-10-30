using Npgsql;
using ProjetoTcc.Models;

namespace ProjetoTcc.Data
{
    public class DepoimentosDB
    {
        private readonly string _connectionString;

        public DepoimentosDB(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? "Data Source=app.db";
            CriarTabelasSeNaoExistem();
        }

        private void CriarTabelasSeNaoExistem()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();

            // Tabela de Depoimentos e Tabela de Admins
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Depoimentos (
                    Id SERIAL PRIMARY KEY,
                    Titulo TEXT NOT NULL,
                    FaseDiagnostico TEXT NOT NULL,
                    Conteudo TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Admins (
                    Id SERIAL PRIMARY KEY,
                    Nome TEXT NOT NULL,
                    Email TEXT NOT NULL UNIQUE,
                    Senha TEXT NOT NULL
                );";
            command.ExecuteNonQuery();

            // Insere o Administrador Padrão se ainda não existir
            command.CommandText = @"
                INSERT INTO Admins (Id, Nome, Email, Senha) 
                VALUES (1, 'Administradora M.A.E', 'admin@mae.com.br', 'maeane#2023')
                ON CONFLICT (Email) DO NOTHING; ";
            command.ExecuteNonQuery();
        }

        public bool ValidarAdmin(string email, string senha)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(1) FROM Admins WHERE Email = @email AND Senha = @senha;";
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@senha", senha);

            return Convert.ToInt32(command.ExecuteScalar()) > 0;
        }

        public bool AlterarSenhaAdmin(string email, string novaSenha)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "UPDATE Admins SET Senha = @novaSenha WHERE Email = @email;";
            command.Parameters.AddWithValue("@novaSenha", novaSenha);
            command.Parameters.AddWithValue("@email", email);

            return command.ExecuteNonQuery() > 0;
        }

        public void ExcluirDepoimento(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Depoimentos WHERE Id = @id;";
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }

        public void Inserir(Depoimentos depoimento)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Depoimentos (Titulo, FaseDiagnostico, Conteudo)
                VALUES (@titulo, @fase, @conteudo);";

            command.Parameters.AddWithValue("@titulo", depoimento.Titulo);
            command.Parameters.AddWithValue("@fase", depoimento.FaseDiagnostico);
            command.Parameters.AddWithValue("@conteudo", depoimento.Conteudo);

            command.ExecuteNonQuery();
        }

        public List<Depoimentos> ObterTodos()
        {
            var lista = new List<Depoimentos>();
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Titulo, FaseDiagnostico, Conteudo FROM Depoimentos ORDER BY Id DESC;";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Depoimentos
                {
                    Id = reader.GetInt32(0),
                    Titulo = reader.GetString(1),
                    FaseDiagnostico = reader.GetString(2),
                    Conteudo = reader.GetString(3)
                });
            }
            return lista;
        }
    }
}