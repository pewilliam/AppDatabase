using Npgsql;

string userId, password;

Console.WriteLine("Efetue o login com o usuário e senha do professor.");
Console.Write("Usuário: ");
userId = Console.ReadLine();
Console.Write("Senha: ");
password = GetMaskedPassword();
Console.WriteLine();

string connectionString = $"Server=aluguelcarros.cxdeckrhjvvc.us-east-1.rds.amazonaws.com;Port=5432;Database=aluguelcarros;User Id={userId};Password={password}";

using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
{
    try
    {
        conn.Open();
        Console.WriteLine("Conexão efetuada com RDS AWS! Pressione enter para continuar...");
        Console.ReadLine();
    }
    catch (Exception ex)
    {
        Console.WriteLine("Erro na conexão com o RDS AWS: " + ex.Message);
    }

    InsertIntoUsuario(conn);
    PerformSelect(conn);
}

static void InsertIntoUsuario(NpgsqlConnection connection)
{
    try
    {
        Console.Clear();
        Console.WriteLine("Inserção na tabela usuario.");
        Console.Write("Digite o login: ");
        string login = Console.ReadLine();
        Console.Write("Digite a senha: ");
        string senha = GetMaskedPassword();

        string insertQuery = "INSERT INTO rentcar.usuario(login, senha) VALUES (@login, @senha)";
        using NpgsqlCommand cmd = new NpgsqlCommand(insertQuery, connection);

        cmd.ExecuteNonQuery();
        Console.WriteLine("Query efetuada com sucesso! Pressione enter para continuar para o SELECT...");
        Console.ReadLine();
    }
    catch (Exception ex)
    {
        Console.WriteLine("Erro na inserção: " + ex.Message);
    }
}

static void PerformSelect(NpgsqlConnection connection)
{
    try
    {
        Console.Clear();
        Console.WriteLine("SELECT na tabela endereços com LEFT JOIN nas tabelas municípios e estados.");
        string selectQuery = "SELECT e.idendereco, e.logradouro, e.numero, e.bairro, m.codibge, m.nome, es.uf FROM rentcar.endereco e " +
            "LEFT JOIN rentcar.municipio m ON m.codibge = e.codmunicipio " +
            "LEFT JOIN rentcar.estado es ON es.id = m.idestado";

        using NpgsqlCommand cmd = new NpgsqlCommand(selectQuery, connection);
        using NpgsqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            Console.WriteLine("Id: " + reader.GetInt32(0));
            Console.WriteLine("Logradouro: " + reader.GetString(1));
            Console.WriteLine("Número: " + reader.GetString(2));
            Console.WriteLine("Bairro: " + reader.GetString(3));
            Console.WriteLine("Código Município: " + reader.GetInt32(4));
            Console.WriteLine("Município: " + reader.GetString(5));
            Console.WriteLine("UF: " + reader.GetString(6));
            Console.WriteLine("---------------------------------");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Erro no SELECT: " + ex.Message);
    }
}


static string GetMaskedPassword()
{
    var pass = string.Empty;
    ConsoleKey key;
    do
    {
        var keyInfo = Console.ReadKey(intercept: true);
        key = keyInfo.Key;

        if (key == ConsoleKey.Backspace && pass.Length > 0)
        {
            Console.Write("\b \b");
            pass = pass[0..^1];
        }
        else if (!char.IsControl(keyInfo.KeyChar))
        {
            Console.Write("*");
            pass += keyInfo.KeyChar;
        }
    } while (key != ConsoleKey.Enter);
    return pass;
}

