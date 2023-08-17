using Npgsql;

Console.WriteLine("Efetue o login com o usuário e senha do professor.");
Console.Write("Usuário: ");
string userId = Console.ReadLine();
Console.Write("Senha: ");
string password = GetMaskedPassword();
Console.WriteLine();

NpgsqlConnection conn = new NpgsqlConnection();
string con = ($"Server=aluguelcarros.cxdeckrhjvvc.us-east-1.rds.amazonaws.com;Port=5432;Database=aluguelcarros;User Id={userId};Password={password}");

try
{
    conn.ConnectionString = con;
    conn.Open();
    Console.WriteLine("Conexão efetuada com RDS AWS! Pressione enter para continuar...");
    Console.ReadLine();
}
catch
{
    Console.WriteLine("Erro na conexão com o RDS AWS...");
}

try
{
    Console.Clear();
    Console.WriteLine("Inserção na tabela usuario.");
    Console.Write("Digite o login: ");
    string login = Console.ReadLine();
    Console.Write("Digite a senha: ");
    string senha = Console.ReadLine();
    string insert = ($"INSERT INTO rentcar.usuario(login, senha) VALUES ('{login}', '{senha}')");
    NpgsqlCommand cmd = new NpgsqlCommand(insert, conn);
    cmd.ExecuteNonQuery();
    Console.WriteLine("Query efetuada com sucesso! Pressione enter para continuar para o SELECT...");
    Console.ReadLine();
}
catch
{
    Console.WriteLine("Erro na inserção!");
}
try
{
    Console.Clear();
    Console.WriteLine("SELECT na tabela endereços com LEFT JOIN nas tabelas municípios e estados.");
    string select = "SELECT e.idendereco, e.logradouro, e.numero, e.bairro , m.codibge, m.nome, es.uf FROM rentcar.endereco e " +
    "LEFT JOIN rentcar.municipio m ON m.codibge = e.codmunicipio " +
    "LEFT JOIN rentcar.estado es ON es.id = m.idestado";
    NpgsqlCommand cmd1 = new NpgsqlCommand(select, conn);
    NpgsqlDataReader reader = cmd1.ExecuteReader();
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
catch
{
    Console.WriteLine("Erro no SELECT!");
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

