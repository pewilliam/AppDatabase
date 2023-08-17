using Npgsql;

NpgsqlConnection conn = new NpgsqlConnection();
string con = "Server=aluguelcarros.cxdeckrhjvvc.us-east-1.rds.amazonaws.com;Port=5432;Database=aluguelcarros;User Id=postgres;Password=pedrow2001";

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
    Console.WriteLine("Erro na query!");
}

try
{
    string select = "SELECT e.idendereco, e.logradouro, e.numero, e.bairro , m.codibge, m.nome FROM rentcar.endereco e LEFT JOIN rentcar.municipio m ON m.codibge = e.codmunicipio";
    NpgsqlCommand cmd = new NpgsqlCommand(select, conn);
    NpgsqlDataReader reader = cmd.ExecuteReader();
    while (reader.Read())
    {
        Console.WriteLine("Id: " + reader.GetInt32(0));
        Console.WriteLine("Logradouro: " + reader.GetString(1));
        Console.WriteLine("Número: " + reader.GetString(2));
        Console.WriteLine("Bairro: " + reader.GetString(3));
        Console.WriteLine("Código Município: " + reader.GetInt32(4));
        Console.WriteLine("Município: " + reader.GetString(5));
        Console.WriteLine();
    }
}
catch
{
    Console.WriteLine("Erro na query!");
}

